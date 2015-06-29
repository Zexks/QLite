using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLite.Objects.QTextSupport
{
    internal class CommandManager
    {
        LimitedStack<UndoableCommand> history;
        int disabledCommands = 0;
        int autoUndoCommands = 0;
        readonly int maxHistoryLength = 200;
        Stack<UndoableCommand> redoStack = new Stack<UndoableCommand>();

        public CommandManager()
        {
            history = new LimitedStack<UndoableCommand>(maxHistoryLength);
        }

        public void ExecuteCommand(Command cmd)
        {
            if (disabledCommands > 0)
                return;

            if (cmd is UndoableCommand)
            {
                (cmd as UndoableCommand).autoUndo = autoUndoCommands > 0;
                history.Push(cmd as UndoableCommand);
            }
            cmd.Execute();
            //
            redoStack.Clear();
        }

        public void Undo()
        {
            if (history.Count > 0)
            {
                var cmd = history.Pop();
                //
                BeginDisableCommands();//prevent text changing into handlers
                try
                {
                    cmd.Undo();
                }
                finally
                {
                    EndDisableCommands();
                }
                //
                redoStack.Push(cmd);
            }

            //undo next autoUndo command
            if (history.Count > 0)
            {
                if (history.Peek().autoUndo)
                    Undo();
            }
        }

        internal void Redo()
        {
            if (redoStack.Count == 0)
                return;
            UndoableCommand cmd;
            BeginDisableCommands();//prevent text changing into handlers
            try
            {
                cmd = redoStack.Pop();
                cmd.tb.Selection.Start = cmd.sel.Start;
                cmd.tb.Selection.End = cmd.sel.End;
                cmd.Execute();
                history.Push(cmd);
            }
            finally
            {
                EndDisableCommands();
            }

            //redo command after autoUndoable command
            if (cmd.autoUndo)
                Redo();
        }

        public void BeginAutoUndoCommands()
        {
            autoUndoCommands++;
        }

        public void EndAutoUndoCommands()
        {
            autoUndoCommands--;
            if (autoUndoCommands == 0)
                if (history.Count > 0)
                    history.Peek().autoUndo = false;
        }

        private void BeginDisableCommands()
        {
            disabledCommands++;
        }

        private void EndDisableCommands()
        {
            disabledCommands--;
        }

        internal void ClearHistory()
        {
            history.Clear();
            redoStack.Clear();
        }

        public bool UndoEnabled
        {
            get
            {
                return history.Count > 0;
            }
        }

        public bool RedoEnabled
        {
            get { return redoStack.Count > 0; }
        }

    }

}
