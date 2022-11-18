namespace Cinnamon.Core.Module.NotificationService.Handler.WelcomeNotifiy;

public class WelcomeNotifyHelper 
{
    public string GetTemplate(string host)
    {
        return $@"
            <div
                style='
                    padding: 3rem;
                    font-size: 16px;
                    font-weight: bold;
                    width: 550px;
                    border: 2px solid;
                    border-radius: 5px;
                    margin: 0 auto;
                    color: #545454;
                    line-height: 1.5;
                '
                >
                <div style='margin-bottom: 2rem'>
                    <img
                    src='{host}/images/cinnamon-logo.png'
                    alt='logo'
                    style='width: 200px; height: auto'
                    />
                </div>
                <p>Hello!</p>
                <p>
                    We're glad you're here! Welcome to Cinnamon, the family experience
                    marketplace!
                </p>
                <p>
                    Cinnamon seeks to create enjoyable and meaningful experiences. We assist
                    families in discovering new and exciting activities, and we assist our
                    Cinnamon Makers in expanding their businesses. Explore our website to view
                    all of the experiences our Cinnamon Makers have to offer and select the one
                    that suits you best! Or become a Cinnamon Maker and earn money while sharing
                    your passion with children and families throughout the world.
                </p>
                <p>
                    You will undoubtedly be one of the first users to learn about planned new
                    features.
                </p>
                <p>Thank you and welcome to the Cinnamon family!</p>
            </div>
        ";
    }
}