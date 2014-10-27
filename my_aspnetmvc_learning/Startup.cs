using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
[assembly: OwinStartupAttribute(typeof(my_aspnetmvc_learning.Startup))]
namespace my_aspnetmvc_learning
{
    public partial class Startup
    {
        private readonly string HangFireDB = @"Data Source=.;Initial Catalog=HangFireDB;User ID=sa;Password=123";
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                config.UseSqlServerStorage(HangFireDB);
                config.UseServer();

            });
            RecurringJob.AddOrUpdate(() => TestJob(), Cron.Daily);
            ConfigureAuth(app);
        }

        public void TestJob()
        {

        }
    }
}
