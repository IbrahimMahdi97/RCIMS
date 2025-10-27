using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Service.Interface;

namespace Service;

public class FirebaseService : IFirebaseService
{
    private readonly FirebaseMessaging _messaging;

    public FirebaseService()
    {
        var app = FirebaseApp.DefaultInstance == null
            ? FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("serviceAccountKey.json")
                    .CreateScoped("https://www.googleapis.com/auth/firebase.messaging")
            })
            : FirebaseApp.DefaultInstance;

        _messaging = FirebaseMessaging.GetMessaging(app);
    }

    public async Task SendNotificationAsync(Message message)
    {
        await _messaging.SendAsync(message);
    }
}