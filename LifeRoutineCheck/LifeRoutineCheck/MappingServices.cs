using LifeRoutineCheck.Areas.Index.Services;
using LifeRoutineCheck.Areas.Index.Services.Imprements;
using LifeRoutineCheck.Areas.Person.Services;
using LifeRoutineCheck.Areas.Person.Services.Imprements;
using LifeRoutineCheck.Areas.Routine.Services;
using LifeRoutineCheck.Areas.Routine.Services.Imprements;
using LifeRoutineCheck.Areas.Task.Services;
using LifeRoutineCheck.Areas.Task.Services.Imprements;
using LifeRoutineCheck.Services;
using LifeRoutineCheck.Services.Imprements;
using Microsoft.Extensions.DependencyInjection;

namespace LifeRoutineCheck
{
    public static class MappingServices
    {
        internal static void ConfigureServices(IServiceCollection service)
        {
            service.AddTransient<ILoginService, LoginService>();
            service.AddTransient<IPersonService, PersonService>();
            service.AddTransient<IIndexService, IndexService>();
            service.AddTransient<IRoutineService, RoutineService>();
            service.AddTransient<ITaskService, TaskService>();
        }
    }
}
