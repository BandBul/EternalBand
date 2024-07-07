using EternalBAND.Common;
using EternalBAND.DomainObjects;

namespace EternalBAND.DomainObjects.ViewModel;

public class ChatViewModel
{
    public MessageBox? CurrentChat { get; set; }
    public List<MessageBox>? AllChat { get; set; }
}