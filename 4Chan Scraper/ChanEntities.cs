namespace _4Chan_Scraper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class ChanEntities : DbContext
    {
        // Your context has been configured to use a 'ChanEntities' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // '_4Chan_Scraper.ChanEntities' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ChanEntities' 
        // connection string in the application configuration file.
        public ChanEntities()
            : base("name=ChanEntities")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<DbThread> DbTheads { get; set; }
    }

    public class DbThread
    {
        public int Id { get; set; }

        [Index]
        [StringLength(200)]
        public string ThreadNumber { get; set; }

        public DateTime DateAdded { get; set; }
    }


    /* 4Chan Objects */
    public class Board4Page
    {
        public string page { get; set; }
        public IEnumerable<Board4Thread> threads { get; set; }
    }

    public class Board4Thread
    {
        public string no { get; set; }
        public string last_modified { get; set; }
    }

    public class Thread4
    {
        public IEnumerable<Thread4Post> posts { get; set; }
    }

    public class Thread4Post
    {
        public string no { get; set; }
        public string filename { get; set; }
        public string tim { get; set; }
        public string ext { get; set; }
    }
    /* END: 4Chan Objects */
}