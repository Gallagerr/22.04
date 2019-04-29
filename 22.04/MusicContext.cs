namespace _22._04
{
  using System;
  using System.Data.Entity;
  using System.Linq;

  public class MusicContext : DbContext
  {
    // Контекст настроен для использования строки подключения "MusicContext" из файла конфигурации  
    // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
    // "_22._04.MusicContext" в экземпляре LocalDb. 
    // 
    // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "MusicContext" 
    // в файле конфигурации приложения.
    public MusicContext()
        : base("name=MusicContext")
    {
    }
    public DbSet<Band> Bands { get; set; }
    public DbSet<Song> Songs { get; set; }
    // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
    // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

    // public virtual DbSet<MyEntity> MyEntities { get; set; }
  }

  //public class MyEntity
  //{
  //    public int Id { get; set; }
  //    public string Name { get; set; }
  //}
}