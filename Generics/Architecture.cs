using System;
using System.Collections.Generic;

namespace Generics.Robots
{
    public interface IRobotAI<out Command>
    {
        Command GetCommand();
    }

    public class ShooterAI : IRobotAI<ShooterCommand>
    {
        int counter = 1;
        public ShooterCommand GetCommand()
        {
            ShooterCommand new_Shoot = ShooterCommand.ForCounter(counter ++);
            return new_Shoot;
        }
    }

    public class BuilderAI : IRobotAI<BuilderCommand>
    {
        int counter = 1;
        public BuilderCommand GetCommand()
        {
            BuilderCommand new_Build = BuilderCommand.ForCounter(counter ++);
            return new_Build;
        }
    }

    public interface Device<in T> : IMoveCommand
    {
        string ExecuteCommand(T command);
    }

    public class Mover : Device<IMoveCommand>
    {
        public Point Destination => throw new NotImplementedException();
        public string ExecuteCommand(IMoveCommand command)
        {
            if (command == null)
                throw new ArgumentException();
            return $"MOV {command.Destination.X}, {command.Destination.Y}";
        }
    }

    public class ShooterMover : Device<IShooterMoveCommand>
    {
        public Point Destination => throw new NotImplementedException();
        public string ExecuteCommand(IShooterMoveCommand command)
        {
            if (command == null)
                throw new ArgumentException();
            var hide = command.ShouldHide ? "YES" : "NO";
            return $"MOV {command.Destination.X}, {command.Destination.Y}, USE COVER {hide}";
        }
    }

    public static class Robot
    {
        public static Robot<TCommand> Create<TCommand>(IRobotAI<TCommand> ai, Device<TCommand> executor)
        {
            return new Robot<TCommand>(ai, executor);
        }
    }

    public class Robot<T>
    {
        public IRobotAI<T> AI;
        public Device<T> Device;
        public Robot(IRobotAI<T> ai, Device<T> executor)
        {
            AI = ai;
            Device = executor;
        }

        public IEnumerable<string> Start(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                T command = AI.GetCommand();
                if (command != null)
                    yield return Device.ExecuteCommand(command);
                
            }
        }
    }

}