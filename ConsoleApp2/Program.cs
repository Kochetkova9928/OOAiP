using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        public int CtoB = 0;
        public int LoopCount = 0;

        public bool Check(IEnumerable<Lexeme> lexemes)
        {
            var stateMachine = new StateMachine();
            foreach (var lexeme in lexemes)
            {
                stateMachine.Handle(lexeme);
                if (!stateMachine.isValid)
                {
                    break;
                }
            }

            CtoB = stateMachine.curCtoB;
            LoopCount = stateMachine.curLoopCount;

            return stateMachine.currentState == new StateZ(stateMachine);
        }

        static void Main(string[] args)
        {
        }
    }

    enum Lexeme
    {
        A,
        B,
        C,
        D
    }

    class StateMachine
    {
        public State currentState;
        public int curCtoB = 0;
        public int curLoopCount = 0;
        public bool isValid = true;

        public StateMachine()
        {
            currentState = new StateA(this);
        }

        public void Handle(Lexeme lexeme)
        {
            currentState.GetNextStateByLexeme(lexeme);
        }
    }

    interface State
    {
        State GetNextStateByLexeme(Lexeme l);
    }

    class StateA : State
    {
        private StateMachine stateMachine;

        public StateA(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public State GetNextStateByLexeme(Lexeme lexeme)
        {
            switch (lexeme)
            {
                case Lexeme.C:
                    Console.WriteLine("From StateA to StateC");
                    return new StateC(stateMachine);
                case Lexeme.D:
                    return new StateB(stateMachine);
                default:
                    stateMachine.isValid = false;
                    return this;
            }
        }
    }

    class StateB : State
    {
        private StateMachine stateMachine;

        public StateB(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public State GetNextStateByLexeme(Lexeme lexeme)
        {
            switch (lexeme)
            {
                case Lexeme.A:
                    return new StateC(stateMachine);
                case Lexeme.D:
                    return new StateA(stateMachine);
                default:
                    stateMachine.isValid = false;
                    return this;
            }
        }
    }

    class StateC : State
    {
        private StateMachine stateMachine;

        public StateC(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public State GetNextStateByLexeme(Lexeme lexeme)
        {
            switch (lexeme)
            {
                case Lexeme.A:
                    stateMachine.curCtoB++;
                    return new StateB(stateMachine);
                case Lexeme.B:
                    stateMachine.curLoopCount++;
                    return new StateC(stateMachine);
                case Lexeme.C:
                    return new StateA(stateMachine);
                case Lexeme.D:
                    return new StateZ(stateMachine);
                default:
                    stateMachine.isValid = false;
                    return this;
            }
        }
    }

    class StateZ : State
    {
        private StateMachine stateMachine;

        public StateZ(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public State GetNextStateByLexeme(Lexeme lexeme)
        {
            stateMachine.isValid = false;
            return this;
        }
    }
}