namespace EternalBAND.Models.ViewModel;

public class ChatViewModel
{
    public MessageBox? CurrentChat { get; set; }
    public List<MessageBox>? AllChat { get; set; }
}


public class MessageBox
{
    public MessageBox(MessageMetadata messageMetadata) : this(messageMetadata, new List<Messages>())
    {
    }

    public MessageBox(MessageMetadata messageMetadata, IList<Messages>? messages)
    {
        MessageMetadata = messageMetadata;
        Messages = messages;
    }

    public MessageMetadata MessageMetadata { get; set; }
    public IList<Messages>? Messages { get; set; }
}



public class MessageMetadata
{

    public MessageMetadata(int postid, string[] recipientsId)
    {
        RecipientsId = recipientsId;
        PostId = postid;
    }

    public int PostId { get; set; }

    public string[] RecipientsId { get; set; }
}