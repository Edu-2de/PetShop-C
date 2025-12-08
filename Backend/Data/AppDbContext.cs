using Microsoft.EntityFrameworkCore;
using SIGA_PET.Models;

namespace SIGA_PET.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<ProdutoImagem> ProdutoImagens { get; set; }
        
        // NOVO: Tabela de relacionamento muitos-para-muitos
        public DbSet<ServicoFuncionario> ServicoFuncionarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações existentes...

            // Configuração Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.PasswordHash).HasMaxLength(256);
                entity.Property(e => e.TipoUsuario).HasMaxLength(20);
            });

            // Configuração Tutor
            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.HasOne(t => t.Usuario)
                      .WithOne()
                      .HasForeignKey<Tutor>(t => t.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração Funcionario
            modelBuilder.Entity<Funcionario>(entity =>
            {
                entity.HasOne(f => f.Usuario)
                      .WithOne()
                      .HasForeignKey<Funcionario>(f => f.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração Animal
            modelBuilder.Entity<Animal>(entity =>
            {
                entity.HasOne(a => a.Tutor)
                      .WithMany(t => t.Animais)
                      .HasForeignKey(a => a.TutorId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // Configuração Servico
            modelBuilder.Entity<Servico>(entity =>
            {
                entity.HasOne(s => s.FuncionarioResponsavel)
                      .WithMany(f => f.ServicosResponsavel)
                      .HasForeignKey(s => s.FuncionarioResponsavelId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // NOVA: Configuração ServicoFuncionario (muitos-para-muitos)
            modelBuilder.Entity<ServicoFuncionario>(entity =>
            {
                // Chave composta
                entity.HasKey(sf => new { sf.ServicoId, sf.FuncionarioId });

                // Relacionamento com Servico
                entity.HasOne(sf => sf.Servico)
                      .WithMany(s => s.ServicoFuncionarios)
                      .HasForeignKey(sf => sf.ServicoId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento com Funcionario
                entity.HasOne(sf => sf.Funcionario)
                      .WithMany(f => f.ServicoFuncionarios)
                      .HasForeignKey(sf => sf.FuncionarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração Agendamento
            modelBuilder.Entity<Agendamento>(entity =>
            {
                entity.HasOne(a => a.Animal)
                      .WithMany(an => an.Agendamentos)
                      .HasForeignKey(a => a.AnimalId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(a => a.Servico)
                      .WithMany(s => s.Agendamentos)
                      .HasForeignKey(a => a.ServicoId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(a => a.Funcionario)
                      .WithMany(f => f.Agendamentos)
                      .HasForeignKey(a => a.FuncionarioId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.Property(a => a.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });

            // Configuração Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasOne(p => p.Categoria)
                      .WithMany(c => c.Produtos)
                      .HasForeignKey(p => p.CategoriaId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Fornecedor)
                      .WithMany(f => f.Produtos)
                      .HasForeignKey(p => p.FornecedorId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasIndex(p => p.CodigoBarras)
                      .IsUnique()
                      .HasFilter("[CodigoBarras] IS NOT NULL");
            });

            // Configuração ItemVenda
            modelBuilder.Entity<ItemVenda>(entity =>
            {
                entity.HasOne(iv => iv.Venda)
                      .WithMany(v => v.Itens)
                      .HasForeignKey(iv => iv.VendaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(iv => iv.Produto)
                      .WithMany(p => p.ItemVendas)
                      .HasForeignKey(iv => iv.ProdutoId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(iv => iv.Servico)
                      .WithMany(s => s.ItemVendas)
                      .HasForeignKey(iv => iv.ServicoId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            // ? Configuração Venda - ADICIONADO RELACIONAMENTO COM USUARIO
            modelBuilder.Entity<Venda>(entity =>
            {
                entity.HasOne(v => v.Tutor)
                      .WithMany()
                      .HasForeignKey(v => v.TutorId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(v => v.Usuario)
                      .WithMany()
                      .HasForeignKey(v => v.UsuarioId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(v => v.Funcionario)
                      .WithMany()
                      .HasForeignKey(v => v.FuncionarioId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuração ProdutoImagem
            modelBuilder.Entity<ProdutoImagem>(entity =>
            {
                entity.HasOne(pi => pi.Produto)
                      .WithMany(p => p.Imagens)
                      .HasForeignKey(pi => pi.ProdutoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}