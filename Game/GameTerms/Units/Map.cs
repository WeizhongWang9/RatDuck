using CardGame.Game.GameTerms.Abilities;
using CardGame.Game.GameTerms.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardGame.Game;

namespace RatDuck.Script.Game.GameTerms.Units
{
    public class Map
    {
        AbilityGameNode abilityGameNode;
        AbilityChaoticMeasure abilityChaoticMeasure;
        List<GameNode> nodes = new List<GameNode>();
        List<gameNodeRelation> relations = new List<gameNodeRelation>();
        int _bioProgress = 0;
        int _turns = 0;
        public int turns { get { return _turns; } }
        public int bioProgress { get { return _bioProgress; } }
        public List<GameNode> getNodes() { return nodes; }
        public List<gameNodeRelation> getRelation() { return relations; }

        GameNode _policeStation;
        public GameNode policeStation
        {
            get { return _policeStation; }
            set
            {
                _policeStation = value;
                nodes.Add(_policeStation);
            }
        }
        GameNode _lab;
        public GameNode lab
        {
            get { return _lab; }
            set
            {
                _lab = value;
                nodes.Add(_lab);
            }
        }
        GameNode _prison;
        public GameNode prison
        {
            get { return _prison; }
            set
            {
                _prison = value;
                nodes.Add(_prison);
            }
        }

        public Map(CardGame.Game.Game game)
        {
          
        }

        public void init(CardGame.Game.Game game)
        {
            abilityGameNode = game.abilityHandle.abilityGameNode;
            abilityChaoticMeasure = game.abilityHandle.abilityChaoticMeasure;
        }

        public bool isSpecialNode(GameNode node)
        {
            return node == policeStation || node == lab || node == prison;
        }
        public void add(params GameNode[] gameNodes)
        {
            foreach (var gameNode in gameNodes)
                if (!nodes.Contains(gameNode))
                {
                    abilityChaoticMeasure.init(gameNode);
                    nodes.Add(gameNode);
                }
        }

        public void neighbor(GameNode gameNodeA, GameNode gameNodeB)
        {
            if (abilityGameNode.addNeighbor(gameNodeA, gameNodeB))
                relations.Add(new gameNodeRelation(gameNodeA, gameNodeB));
        }

        public int getChaoticMeasure(GameNode gameNode)
        {
            return abilityChaoticMeasure[gameNode];
        }
        public int getTotalChaoticMeasure()
        {
            var total = 0;
            foreach (GameNode node in nodes)
            {
                if(node != lab && node != prison && node != policeStation)
                    total += getChaoticMeasure(node);
            }
            return total;
        }
        public void addBioProgess(int value)
        {
            _bioProgress += value;
        }

        public void nextTurn()
        {
            _turns += 1;
        }

    }
}
