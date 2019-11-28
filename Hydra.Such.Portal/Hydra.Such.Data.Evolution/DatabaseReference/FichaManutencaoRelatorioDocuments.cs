using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.DatabaseReference
{
    public partial class FichaManutencaoRelatorioDocuments
    {
        public int Id { get; set; }
        public int IdRelatorio { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string FileOriginalName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
