using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TaskMonMobile
{
    public enum DeepLinkType
    {
        Survey,
        Group
    }
    
    public class DeepLinkMessage : ValueChangedMessage<(DeepLinkType Type, string Id)>
    {
        public DeepLinkMessage(DeepLinkType type, string id) : base((type, id))
        {
        }
    }
}