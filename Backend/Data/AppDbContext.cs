using Microsoft.EntityFrameworkCore;
using SIGA_PET.Models;

namespace SIGA_PET.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<RegistroProntuario> RegistrosProntuario { get; set; }
        public DbSet<ProdutoImagem> ProdutoImagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversão de Enum para String no banco
            modelBuilder.Entity<Agendamento>()
                .Property(a => a.Status)
                .HasConversion<string>();

            // =============================================================
            // CORREÇÃO DO ERRO DE CICLO/MULTIPLOS CAMINHOS (Cascade Paths)
            // =============================================================

            // Usuario -> Tutor (1:1)
            modelBuilder.Entity<Tutor>()
                .HasOne(t => t.Usuario)
                .WithOne(u => u.Tutor)
                .HasForeignKey<Tutor>(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // <--- OBRIGATÓRIO SER RESTRICT

            // Usuario -> Funcionario (1:1)
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Usuario)
                .WithOne(u => u.Funcionario)
                .HasForeignKey<Funcionario>(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // <--- OBRIGATÓRIO SER RESTRICT

            // =============================================================

            // Categoria -> Produto (1:N)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Tutor -> Animal (1:N)
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Tutor).WithMany(t => t.Animais).HasForeignKey(a => a.TutorId).OnDelete(DeleteBehavior.Cascade);

            // Animal -> Agendamento (1:N)
            modelBuilder.Entity<Agendamento>()
                .HasOne(ag => ag.Animal).WithMany(an => an.Agendamentos).HasForeignKey(ag => ag.AnimalId).OnDelete(DeleteBehavior.Cascade);

            // Funcionario -> Agendamento (1:N)
            modelBuilder.Entity<Agendamento>()
                .HasOne(ag => ag.Funcionario).WithMany(f => f.Agendamentos).HasForeignKey(ag => ag.FuncionarioId).OnDelete(DeleteBehavior.SetNull);

            // Servico -> Agendamento (1:N)
            modelBuilder.Entity<Agendamento>()
                .HasOne(ag => ag.Servico).WithMany(s => s.Agendamentos).HasForeignKey(ag => ag.ServicoId).OnDelete(DeleteBehavior.Restrict);

            // Animal -> RegistroProntuario (1:N)
            modelBuilder.Entity<RegistroProntuario>()
                .HasOne(rp => rp.Animal).WithMany(a => a.Registros).HasForeignKey(rp => rp.AnimalId).OnDelete(DeleteBehavior.Cascade);

            // Funcionario -> RegistroProntuario (1:N)
            modelBuilder.Entity<RegistroProntuario>()
                .HasOne(rp => rp.Funcionario).WithMany(f => f.Registros).HasForeignKey(rp => rp.FuncionarioId).OnDelete(DeleteBehavior.SetNull);

            // Fornecedor -> Produto (1:N)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fornecedor).WithMany(f => f.Produtos).HasForeignKey(p => p.FornecedorId).OnDelete(DeleteBehavior.SetNull);

            // Tutor -> Venda (1:N)
            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Tutor).WithMany(t => t.Vendas).HasForeignKey(v => v.TutorId).OnDelete(DeleteBehavior.SetNull);

            // Funcionario -> Venda (1:N)
            modelBuilder.Entity<Venda>()
                .HasOne(v => v.Funcionario).WithMany(f => f.Vendas).HasForeignKey(v => v.FuncionarioId).OnDelete(DeleteBehavior.SetNull);

            // Venda -> ItemVenda (1:N)
            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Venda).WithMany(v => v.Itens).HasForeignKey(iv => iv.VendaId).OnDelete(DeleteBehavior.Cascade);

            // Produto -> ItemVenda (1:N)
            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Produto).WithMany(p => p.ItemVendas).HasForeignKey(iv => iv.ProdutoId).OnDelete(DeleteBehavior.SetNull);

            // Servico -> ItemVenda (1:N)
            modelBuilder.Entity<ItemVenda>()
                .HasOne(iv => iv.Servico).WithMany(s => s.ItemVendas).HasForeignKey(iv => iv.ServicoId).OnDelete(DeleteBehavior.SetNull);

            // Produto -> ProdutoImagem (1:N)
            modelBuilder.Entity<ProdutoImagem>()
                .HasOne(pi => pi.Produto).WithMany(p => p.Imagens).HasForeignKey(pi => pi.ProdutoId).OnDelete(DeleteBehavior.Cascade);

            // Índices
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Produto>().HasIndex(p => p.CodigoBarras);
        }
    }
}