using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;

namespace iCrabee.WebPortal.Models
{
    public class ListByTagIdViewModel
    {
        public Pagination<KnowledgeBaseQuickVM> Data { set; get; }

        public LabelVM LabelVm { set; get; }
    }
}
