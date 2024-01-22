using EternalBAND.Common;
using EternalBAND.DomainObjects.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalBAND.DomainObjects
{
    public static class Initials
    {
        public static Dictionary<PostStatus, PostStatusViewModel> InitialPostStatusViewModelValue = 
            new Dictionary<PostStatus, PostStatusViewModel>()
        {
            { PostStatus.Active, new PostStatusViewModel()
                {
                    DisplayText = "Aktif",
                    Color = "green",
                    HeaderDisplayText = "Aktif"
                }
            },
            { PostStatus.PendingApproval, new PostStatusViewModel()
                {
                    DisplayText = "Onay Bekliyor",
                    Color = "orange",
                    HeaderDisplayText = "Onay Bekleyen"
                }
            },
            { PostStatus.DeActive, new PostStatusViewModel()
                {
                    DisplayText = "Arşivde",
                    Color = "purple",
                    HeaderDisplayText = "Arşivlenen"
                }
            }
        };

    }
}
