using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Harald.IntegrationTests.Util
{
    public class Gherkin
    {
        public static async void RunAsync(params Func<Task>[] actions)
        {
            foreach (var task in actions)
            {
                Console.WriteLine($"Running: {task.GetMethodInfo().Name}");
                await Task.Run(task);
            }
        }
        
    }

    public class Gherkin2
    {
        private List<TaskContainer> tasks;

        public Gherkin2(List<TaskContainer> tasks)
        {
            this.tasks = tasks;
        }
        
        public async Task RunAsync(params Func<Task>[] actions)
        {
            foreach (var task in tasks)
            {
                Console.WriteLine($"Running: {task.Action} : {task.Func.GetMethodInfo().Name}");
                await Task.Run(task.Func);
            }
        }
    }

    public enum Action
    {
        Given,
        When,
        Then,
        But,
        And
    }

    public class TaskContainer
    {
        public Action Action { get; set; }
        public Func<Task> Func { get; set; }

        public TaskContainer(Action action, Func<Task> func)
        {
            Action = action;
            Func = func;
        }
    }

    public class Gherkin2Builder
    {
        private Gherkin2 _payload;
        private List<TaskContainer> tasks;

        public Gherkin2Builder()
        {
            tasks = new List<TaskContainer>();
        }

        public static Gherkin2Builder New()
        {
            return new Gherkin2Builder();
        }

        public Gherkin2Builder Given(Func<Task> func)
        {
            tasks.Add(new TaskContainer(Action.Given, func));
            return this;
        }
        
        public Gherkin2Builder When(Func<Task> func)
        {
            tasks.Add(new TaskContainer(Action.When, func));
            return this;
        }
        
        public Gherkin2Builder Then(Func<Task> func)
        {
            tasks.Add(new TaskContainer(Action.Then, func));
            return this;
        }
        
        public Gherkin2Builder But(Func<Task> func)
        {
            tasks.Add(new TaskContainer(Action.But, func));
            return this;
        }
        
        public Gherkin2Builder And(Func<Task> func)
        {
            tasks.Add(new TaskContainer(Action.And, func));
            return this;
        }

        public Gherkin2 Build()
        {
            _payload = new Gherkin2(tasks);
            return _payload;
        }
    }
}