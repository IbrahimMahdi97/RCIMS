using FirebaseAdmin.Messaging;

namespace Service.Interface;

public interface IFirebaseService
{
    Task SendNotificationAsync(Message message);
}