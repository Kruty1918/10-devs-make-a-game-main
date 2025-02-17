using System.Collections.Generic;
using System.Linq;

namespace Bonjoura
{
    public static class GameStates
    {
        public static GameState State { get; private set; } = GameState.Played;

        private static readonly IValidator<GameState> validator;

        static GameStates()
        {
            validator = new StateValidator();
        }

        public static void SetState(GameState newState)
        {
            if (validator.Validate(newState)) State = newState;
        }
    }

    public interface IValidator<T>
    {
        bool Validate(T arg);
    }

    /// <summary>
    /// Validates state transitions for the <see cref="GameState"/> enumeration.
    /// This class checks whether the requested state change is allowed based on the current game state.
    /// </summary>
    public class StateValidator : IValidator<GameState>
    {
        private readonly Dictionary<GameState, GameState[]> validTransitions;

        public StateValidator()
        {
            validTransitions = new Dictionary<GameState, GameState[]>
        {
            { GameState.Played, new[] { GameState.Paused } },
            { GameState.Paused, new[] { GameState.Played } },
            { GameState.Dead, new[] { GameState.Played, GameState.Paused } }
        };
        }

        public StateValidator(Dictionary<GameState, GameState[]> validTransitions)
        {
            this.validTransitions = validTransitions;
        }

        /// <summary>
        /// Validates whether transitioning to the specified new state is allowed based on the current game state.
        /// </summary>
        /// <param name="newState">The new game state to transition to.</param>
        /// <returns>True if the state transition is valid, otherwise false.</returns>
        public bool Validate(GameState newState)
        {
            GameState currentState = GameStates.State;

            // Define allowed transitions


            // Check if the new state is a valid transition from the current state
            return validTransitions.TryGetValue(newState, out var validPreviousStates) &&
                   validPreviousStates.Contains(currentState);
        }
    }
}