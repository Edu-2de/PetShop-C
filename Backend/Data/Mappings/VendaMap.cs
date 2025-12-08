using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGA_PET.Models;

namespace SIGA_PET.Data.Mappings
{
    public class VendaMap : IEntityTypeConfiguration<Venda>
    {
        public void Configure(EntityTypeBuilder<Venda> builder)
        {
            builder.HasKey(v => v.VendaId);

            builder.Property(v => v.ValorTotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(v => v.FormaPagamento)
                .HasMaxLength(50);

            builder.Property(v => v.Observacoes)
                .HasMaxLength(500);

            // Relacionamento com Tutor (1 Venda pertence a 1 Tutor)
            builder.HasOne(v => v.Tutor)
                .WithMany(t => t.Vendas)
                .HasForeignKey(v => v.TutorId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relacionamento com Usuario (1 Venda pertence a 1 Usuario)
            builder.HasOne(v => v.Usuario)
                .WithMany(u => u.Vendas)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relacionamento com Funcionario (1 Venda é registrada por 1 Funcionario)
            builder.HasOne(v => v.Funcionario)
                .WithMany(f => f.Vendas)
                .HasForeignKey(v => v.FuncionarioId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
