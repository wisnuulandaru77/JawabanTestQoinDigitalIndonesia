namespace MainService;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Test01Configuration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Test01> Test01s { get; set; }
}

public class Test01Configuration : IEntityTypeConfiguration<Test01>
{
    public void Configure(EntityTypeBuilder<Test01> builder)
    {
        builder.ToTable(nameof(Test01));
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedOnAdd();
        builder.Property(item => item.Nama).HasMaxLength(100);
        builder.Property(item => item.Status).IsRequired(false);
        builder.Property(item => item.Created).IsRequired(false);
        builder.Property(item => item.Updated).IsRequired(false);
    }
}
