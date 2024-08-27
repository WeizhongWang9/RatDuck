using CardGame.Game.GameTerms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
namespace CardGame.Game.Utilities
{
    delegate bool filter(Target target, out string errorStr);
    public interface IOrder<T>
    {
        bool filter(Target target, out string errorStr);
        bool activationCondition(LinkedList<Target> targets, out string errorStr);
        void skillEffect(T order);
    }
    public enum OrderType {Unit,Player, GameNode}
    
    public class Target
    {
        Unit _unit = null;
        Player _player = null;
        GameNode _gameNode = null;

        OrderType orderType;

        public Target(Unit unit) { this._unit = unit; orderType = OrderType.Unit; }
        public Target(Player player) { this._player = player; orderType = OrderType.Player; }
        public Target(GameNode gameNode) { this._gameNode = gameNode; orderType = OrderType.GameNode; }

        public Unit unit => _unit;
        public Player player => _player;
        public GameNode gameNode => _gameNode;
        public OrderType getOrderType()
        {
            return orderType;
        }

    }
}
*/