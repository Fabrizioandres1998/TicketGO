using Microsoft.EntityFrameworkCore;
using TicketsGO.Models;

namespace TicketsGO.Data
{
    public class AppDbContext : DbContext
    {
        // ============================================================
        // TABLAS
        // ============================================================
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Historial> Historiales { get; set; }
        public DbSet<Transferencia> Transferencias { get; set; }

        // ============================================================
        // CONSTRUCTOR
        // ============================================================
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ============================================================
        // CONFIGURACIÓN DE MODELOS
        // ============================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurarUsuario(modelBuilder);
            ConfigurarEvento(modelBuilder);
            ConfigurarTicket(modelBuilder);
            ConfigurarCompra(modelBuilder);
            ConfigurarTransferencia(modelBuilder);
            ConfigurarHistorial(modelBuilder);
        }

        // ============================================================
        // USUARIO
        // ============================================================
        private static void ConfigurarUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entidad =>
            {
                entidad.ToTable("usuario");
                entidad.HasKey(u => u.Id);

                entidad.Property(u => u.Id).HasColumnName("id_usuario");
                entidad.Property(u => u.Nombre).HasColumnName("nombre").HasMaxLength(100);
                entidad.Property(u => u.Apellido).HasColumnName("apellido").HasMaxLength(100);
                entidad.Property(u => u.Email).HasColumnName("email").HasMaxLength(150);
                entidad.Property(u => u.Password).HasColumnName("password").HasMaxLength(255);
                entidad.Property(u => u.Rol)
                    .HasColumnName("rol")
                    .HasConversion<string>()
                    .HasMaxLength(30);
                entidad.Property(u => u.Estado).HasColumnName("estado");
                entidad.Property(u => u.Telefono).HasColumnName("telefono").HasMaxLength(30);
                entidad.Property(u => u.EmailVerificado).HasColumnName("email_verificado");
                entidad.Property(u => u.VerificationToken)
                    .HasColumnName("verification_token")
                    .HasMaxLength(255);
                entidad.Property(u => u.ResetPasswordExpires)
                    .HasColumnName("reset_password_expires")
                    .HasColumnType("DATETIME");

                entidad.HasIndex(u => u.Email).IsUnique();
            });
        }

        // ============================================================
        // EVENTO
        // ============================================================
        private static void ConfigurarEvento(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evento>(entidad =>
            {
                entidad.ToTable("evento");
                entidad.HasKey(e => e.Id);

                entidad.Property(e => e.Id).HasColumnName("id_evento");
                entidad.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entidad.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(150);
                entidad.Property(e => e.Descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("TEXT");
                entidad.Property(e => e.FechaInicio)
                    .HasColumnName("fecha_inicio")
                    .HasColumnType("DATETIME");
                entidad.Property(e => e.FechaFin)
                    .HasColumnName("fecha_fin")
                    .HasColumnType("DATETIME");
                entidad.Property(e => e.Provincia).HasColumnName("provincia").HasMaxLength(100);
                entidad.Property(e => e.Ciudad).HasColumnName("ciudad").HasMaxLength(100);
                entidad.Property(e => e.Establecimiento).HasColumnName("establecimiento").HasMaxLength(150);
                entidad.Property(e => e.Privado).HasColumnName("privado");
                entidad.Property(e => e.TicketsTotales).HasColumnName("tickets_totales");
                entidad.Property(e => e.TicketsVendidos).HasColumnName("tickets_vendidos");
                entidad.Property(e => e.PrecioTicket)
                    .HasColumnName("precio_ticket")
                    .HasPrecision(10, 2);
                entidad.Property(e => e.Estado)
                    .HasColumnName("estado")
                    .HasConversion<string>()
                    .HasMaxLength(30);
                entidad.Property(e => e.ImagenUrl)
                    .HasColumnName("imagen_url")
                    .HasMaxLength(255);

                entidad.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // ============================================================
        // TICKET
        // ============================================================
        private static void ConfigurarTicket(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entidad =>
            {
                entidad.ToTable("ticket");
                entidad.HasKey(t => t.Id);

                entidad.Property(t => t.Id).HasColumnName("id_ticket");
                entidad.Property(t => t.IdEvento).HasColumnName("id_evento");
                entidad.Property(t => t.IdCompra).HasColumnName("id_compra");
                entidad.Property(t => t.IdUsuario).HasColumnName("id_usuario");
                entidad.Property(t => t.Estado)
                    .HasColumnName("estado")
                    .HasConversion<string>()
                    .HasMaxLength(30);
                entidad.Property(t => t.Codigo).HasColumnName("codigo").HasMaxLength(100);

                entidad.HasOne(t => t.Evento)
                    .WithMany()
                    .HasForeignKey(t => t.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(t => t.Compra)
                    .WithMany()
                    .HasForeignKey(t => t.IdCompra)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(t => t.Usuario)
                    .WithMany()
                    .HasForeignKey(t => t.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // ============================================================
        // COMPRA
        // ============================================================
        private static void ConfigurarCompra(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Compra>(entidad =>
            {
                entidad.ToTable("compra");
                entidad.HasKey(c => c.Id);

                entidad.Property(c => c.Id).HasColumnName("id_compra");
                entidad.Property(c => c.IdEvento).HasColumnName("id_evento");
                entidad.Property(c => c.IdUsuario).HasColumnName("id_usuario");
                entidad.Property(c => c.TotalTickets).HasColumnName("total_tickets");
                entidad.Property(c => c.MontoTotal)
                    .HasColumnName("monto_total")
                    .HasPrecision(10, 2);
                entidad.Property(c => c.Estado)
                    .HasColumnName("estado")
                    .HasConversion<string>()
                    .HasMaxLength(30);

                entidad.HasOne(c => c.Evento)
                    .WithMany()
                    .HasForeignKey(c => c.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(c => c.Usuario)
                    .WithMany()
                    .HasForeignKey(c => c.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // ============================================================
        // TRANSFERENCIA
        // ============================================================
        private static void ConfigurarTransferencia(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transferencia>(entidad =>
            {
                entidad.ToTable("transferencia");
                entidad.HasKey(t => t.Id);

                entidad.Property(t => t.Id).HasColumnName("id_transferencia");
                entidad.Property(t => t.IdTicket).HasColumnName("id_ticket");
                entidad.Property(t => t.IdUsuarioOrigen).HasColumnName("id_usuario_origen");
                entidad.Property(t => t.IdUsuarioDestino).HasColumnName("id_usuario_destino");
                entidad.Property(t => t.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("DATETIME");

                entidad.HasOne(t => t.Ticket)
                    .WithMany()
                    .HasForeignKey(t => t.IdTicket)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(t => t.UsuarioOrigen)
                    .WithMany()
                    .HasForeignKey(t => t.IdUsuarioOrigen)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(t => t.UsuarioDestino)
                    .WithMany()
                    .HasForeignKey(t => t.IdUsuarioDestino)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        // ============================================================
        // HISTORIAL
        // ============================================================
        private static void ConfigurarHistorial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Historial>(entidad =>
            {
                entidad.ToTable("historial");
                entidad.HasKey(h => h.Id);

                entidad.Property(h => h.Id).HasColumnName("id_historial");
                entidad.Property(h => h.IdEvento).HasColumnName("id_evento");
                entidad.Property(h => h.IdUsuario).HasColumnName("id_usuario");
                entidad.Property(h => h.Descripcion)
                    .HasColumnName("descripcion")
                    .HasColumnType("TEXT");

                entidad.HasOne(h => h.Evento)
                    .WithMany()
                    .HasForeignKey(h => h.IdEvento)
                    .OnDelete(DeleteBehavior.Restrict);

                entidad.HasOne(h => h.Usuario)
                    .WithMany()
                    .HasForeignKey(h => h.IdUsuario)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}