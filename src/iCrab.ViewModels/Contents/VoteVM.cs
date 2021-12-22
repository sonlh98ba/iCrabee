using System;

namespace iCrabee.ViewModels.Contents
{
    public class VoteVM
    {
        public int KnowledgeBaseId { get; set; }
        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
