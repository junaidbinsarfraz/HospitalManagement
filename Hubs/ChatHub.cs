using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HospitalManagament.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Mvc;

namespace HospitalManagament.Hubs
{
    public class ChatHub : Hub
    {
        // Hold all the connections of each user
        public static ConcurrentDictionary<string, UserConnection> UserConnections = new ConcurrentDictionary<string, UserConnection>();
        public void Hello()
        {
            Clients.All.hello();
        }

        // Called this function when user is connected
        public override Task OnConnected()
        {
            lock (UserConnections)
            {
                UserConnections.TryAdd(Context.ConnectionId, new UserConnection() { ConnectionId = Context.ConnectionId });
            }
            return base.OnConnected();
        }

        // Called this fucntion when user is disconnected
        public override Task OnDisconnected(bool StopCalled)
        {
            UserConnection garbage;

            lock (UserConnections)
            {
                UserConnections.TryRemove(Context.ConnectionId, out garbage);

                Clients.Clients(UserConnections.Keys.ToList()).UserDisconnected(
                    JsonConvert.SerializeObject(garbage, Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
            }
            return base.OnDisconnected(StopCalled);
        }

        // Call this function when user send message to other user
        public void SendMessage(int ToUserId, string Text, string ToConnectionId)
        {
            int FromUserId = -1;
            // Get User from Dictionary
            UserConnection Connection;

            UserConnections.TryGetValue(Context.ConnectionId, out Connection);

            HospitalManagementContext db = new HospitalManagementContext();

            // Save message into database
            if (Connection != null)
            {
                // User is online
                User user = Connection.User;
                // Get Patient and Doctor from database
                Patient patient = new Patient();
                Doctor doctor = new Doctor();

                if (Connection.IsPatient)
                {
                    patient = user.Patient;
                    User doctorUser = db.Users.Include(u => u.Doctor).Where(u => u.Id == ToUserId).FirstOrDefault();

                    if (doctorUser != null)
                    {
                        FromUserId = (int)doctorUser.Id;
                        doctor = doctorUser.Doctor;
                    }
                }
                else
                {
                    doctor = user.Doctor;
                    User patientUser = db.Users.Include(u => u.Patient).Where(u => u.Id == ToUserId).FirstOrDefault();

                    if (patientUser != null)
                    {
                        FromUserId = (int)patientUser.Id;
                        patient = patientUser.Patient;
                    }
                }

                if (patient != null && patient.Id != 0 && doctor != null && doctor.Id != 0)
                {
                    db.Messages.Add(new Message() { Patient = patient, Doctor = doctor, Text = Text });

                    db.SaveChanges();

                    // Send Message to user
                    if (FromUserId != -1)
                    {
                        Clients.Client(ToConnectionId).AppendNewMessage(FromUserId, Text);
                    }
                }

            }
            else
            {
                // User is offline
            }
        }

        // Register new user connection
        public void RegisterUser(int UserId, bool IsPatient)
        {
            // Get User from database
            HospitalManagementContext db = new HospitalManagementContext();

            User User;

            if (IsPatient)
            {
                User = db.Users.Include(u => u.Patient).Where(u => u.Id == UserId).FirstOrDefault();
            }
            else
            {
                User = db.Users.Include(u => u.Doctor).Where(u => u.Id == UserId).FirstOrDefault();
            }

            if (User != null)
            {
                UserConnection UserConnection = new UserConnection() { ConnectionId = Context.ConnectionId, IsPatient = IsPatient, User = User };

                Clients.Client(Context.ConnectionId).Init(
                    JsonConvert.SerializeObject(UserConnections.ToList(), Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

                lock (UserConnections)
                {
                    Clients.Clients(UserConnections.Keys.ToList()).NewClientRegistered(
                        JsonConvert.SerializeObject(UserConnection, Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));

                    UserConnections.AddOrUpdate(Context.ConnectionId, UserConnection, (key, oldValue) => UserConnection);
                }
            }
        }

        // Get connection by connection id
        public void GetConnection(string ConnectionId)
        {
            UserConnection userConnection;

            lock (UserConnections)
            {
                UserConnections.TryGetValue(ConnectionId, out userConnection);
            }

            if (userConnection != null)
            {

                Clients.Client(Context.ConnectionId).GetConnectionResult(
                    JsonConvert.SerializeObject(userConnection, Formatting.None,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                }));
            }

        }

        // Send message from one user to other user
        public void SendMessage(string Text, int PatientId, int DoctorId, bool FromPatient, string ConnectionId)
        {
            HospitalManagementContext db = new HospitalManagementContext();

            db.Messages.Add(new Message() { DoctorId = DoctorId, FromPatient = FromPatient, PatientId = PatientId, Text = Text });

            db.SaveChanges();

            User user;

            if (FromPatient)
            {
                user = db.Users.Include(u => u.Patient).Where(u => u.Patient.Id == PatientId).FirstOrDefault();
            }
            else
            {
                user = db.Users.Include(u => u.Doctor).Where(u => u.Doctor.Id == DoctorId).FirstOrDefault();
            }

            Clients.Client(ConnectionId).NewMessageRecieved(Context.ConnectionId, Text, JsonConvert.SerializeObject(user, Formatting.None,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                }));
        }
    }
}