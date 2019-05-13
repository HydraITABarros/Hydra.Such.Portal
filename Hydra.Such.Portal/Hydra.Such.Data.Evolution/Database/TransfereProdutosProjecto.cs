using System;
using System.Collections.Generic;

namespace Hydra.Such.Data.Evolution.Database
{
    public partial class TransfereProdutosProjecto
    {
        public byte[] Timestamp { get; set; }
        public int MovimentoOrigem { get; set; }
        public string ProjectoOrigem { get; set; }
        public string ProdutoOrigem { get; set; }
        public decimal QuantidadeOrigem { get; set; }
        public string ProjectoDestino { get; set; }
        public string ProdutoDestino { get; set; }
        public decimal QuantidadeDestino { get; set; }
        public decimal QuantidadeJaTransferida { get; set; }
        public decimal QtdPrevPossivelTransferir { get; set; }
        public decimal SomaQtdProduto { get; set; }
        public decimal QtdPossivelTransferir { get; set; }
        public string NºDocumento { get; set; }
        public string DescProduto { get; set; }
        public string UnidadeMedida { get; set; }
        public string CodLocaliz { get; set; }
    }
}
