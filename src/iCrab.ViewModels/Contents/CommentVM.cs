using System;
using System.Collections.Generic;

namespace iCrabee.ViewModels.Contents
{
    public class CommentVM
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int KnowledgeBaseId { get; set; }

        public string KnowledgeBaseTitle { get; set; }

        public string KnowledgeBaseSeoAlias { get; set; }

        public string OwnerUserId { get; set; }

        public string OwnerName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public int? ReplyId { get; set; }

        public Pagination<CommentVM> Children { get; set; } = new Pagination<CommentVM>();
    }
}
