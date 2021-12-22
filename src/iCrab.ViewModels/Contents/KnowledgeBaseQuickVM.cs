﻿using System;

namespace iCrabee.ViewModels.Contents
{
    public class KnowledgeBaseQuickVM
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryAlias { get; set; }

        public string CategoryName { get; set; }

        public string Title { get; set; }

        public string SeoAlias { get; set; }

        public string Description { get; set; }

        public int? ViewCount { get; set; } = 0;

        public DateTime CreateDate { get; set; }

        public int? NumberOfVotes { get; set; } = 0;

        public int? NumberOfComments { get; set; } = 0;
    }
}
