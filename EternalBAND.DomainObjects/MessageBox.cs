using EternalBAND.DomainObjects.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.DomainObjects
{
    public class MessageBox : IEntity
    {
        public int Id { get; set; }
        public string Recipient1 { get; set; }
        public string Recipient2 { get; set; }
        public int? PostId { get; set; }
        public int? PostIdBackup { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual Posts Post { get; set; }
        public string PostTitle { get; set; }
        public virtual IList<Messages>? Messages { get; set; }
        public bool IsPostDeleted { get; set; }

        public bool IsRecipient(string userId)
        {
            return Recipient1 == userId || Recipient2 == userId;
        }
        public static Expression<Func<MessageBox, bool>> IsRecipientPredicate(string userId)
        {
            Expression<Func<MessageBox, bool>> predicate = (ms) => ms.Recipient1 == userId || ms.Recipient2 == userId;
            return predicate;
        }

        public static Expression<Func<MessageBox, bool>> IsMessageBoxExistPredicate(string receiverUserId, string senderUserId, int postId, bool isPostExist)
        {
            if (isPostExist)
            {
                    return (ms) =>
                (ms.Recipient1 == receiverUserId || ms.Recipient1 == senderUserId)
                && (ms.Recipient2 == receiverUserId || ms.Recipient2 == senderUserId)
                && (ms.PostId == postId);
            }

            return (ms) =>
                (ms.Recipient1 == receiverUserId || ms.Recipient1 == senderUserId)
                && (ms.Recipient2 == receiverUserId || ms.Recipient2 == senderUserId)
                && (ms.PostIdBackup == postId); ;
        }

        public string GetAnotherRecipient(string recipientId)
        {
            return Recipient1 != recipientId ? Recipient1 : Recipient2;
        }
    }
}
