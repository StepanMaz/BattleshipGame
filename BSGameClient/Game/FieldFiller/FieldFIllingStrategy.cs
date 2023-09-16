using BattleShipGameClient.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSGameClient.Game.FieldFiller
{
    internal class FieldFIllingStrategy : IFieldFillingStrategy
    {
        private IEnumerator<IEnumerable<ShipData>> enumerator;

        public FieldFIllingStrategy()
        {
            enumerator = new RandomPresetIterator(new IEnumerable<ShipData>[]
            {
                new ShipData[] { 
                    new() { Place = new Point(3, 0), Length = 1 },
                    new() { Place = new Point(6, 5), Length = 1 },
                    new() { Place = new Point(8, 5), Length = 1 },
                    new() { Place = new Point(9, 8), Length = 1 },

                    new() { Place = new Point(0, 4), Length = 2, Direction = Direction.TopToBottom },
                    new() { Place = new Point(3, 3), Length = 2, Direction = Direction.LeftToRight },
                    new() { Place = new Point(8, 3), Length = 2, Direction = Direction.TopToBottom },

                    new() { Place = new Point(5, 1), Length = 3, Direction = Direction.TopToBottom },
                    new() { Place = new Point(0, 7), Length = 3, Direction = Direction.LeftToRight },

                    new() { Place = new Point(2, 7), Length = 4, Direction = Direction.TopToBottom }
                },
                new ShipData[] {
                    new() { Place = new Point(0, 9), Length = 1 },
                    new() { Place = new Point(3, 9), Length = 1 },
                    new() { Place = new Point(7, 9), Length = 1 },
                    new() { Place = new Point(6, 5), Length = 1 },

                    new() { Place = new Point(8, 4), Length = 2, Direction = Direction.TopToBottom },
                    new() { Place = new Point(3, 3), Length = 2, Direction = Direction.LeftToRight },
                    new() { Place = new Point(0, 3), Length = 2, Direction = Direction.TopToBottom },

                    new() { Place = new Point(9, 7), Length = 3, Direction = Direction.LeftToRight },
                    new() { Place = new Point(6, 2), Length = 3, Direction = Direction.TopToBottom },

                    new() { Place = new Point(1, 0), Length = 4, Direction = Direction.TopToBottom }
                },
                new ShipData[] {
                    new() { Place = new Point(0, 9), Length = 1 },
                    new() { Place = new Point(3, 9), Length = 1 },
                    new() { Place = new Point(7, 9), Length = 1 },
                    new() { Place = new Point(6, 5), Length = 1 },

                    new() { Place = new Point(8, 4), Length = 2, Direction = Direction.TopToBottom },
                    new() { Place = new Point(3, 3), Length = 2, Direction = Direction.LeftToRight },
                    new() { Place = new Point(0, 3), Length = 2, Direction = Direction.TopToBottom },

                    new() { Place = new Point(9, 7), Length = 3, Direction = Direction.LeftToRight },
                    new() { Place = new Point(6, 2), Length = 3, Direction = Direction.TopToBottom },

                    new() { Place = new Point(1, 0), Length = 4, Direction = Direction.TopToBottom }
                },
                new ShipData[] {
                    new() { Place = new Point(0, 0), Length = 1 },
                    new() { Place = new Point(3, 0), Length = 1 },
                    new() { Place = new Point(7, 0), Length = 1 },
                    new() { Place = new Point(6, 6), Length = 1 },

                    new() { Place = new Point(8, 5), Length = 2, Direction = Direction.TopToBottom },
                    new() { Place = new Point(3, 4), Length = 2, Direction = Direction.LeftToRight },
                    new() { Place = new Point(0, 4), Length = 2, Direction = Direction.TopToBottom },

                    new() { Place = new Point(9, 7), Length = 3, Direction = Direction.LeftToRight },
                    new() { Place = new Point(6, 3), Length = 3, Direction = Direction.TopToBottom },

                    new() { Place = new Point(1, 2), Length = 4, Direction = Direction.TopToBottom }
                },
                new ShipData[] {
                    new() { Place = new Point(1, 0), Length = 1 },
                    new() { Place = new Point(4, 0), Length = 1 },
                    new() { Place = new Point(8, 0), Length = 1 },
                    new() { Place = new Point(7, 6), Length = 1 },

                    new() { Place = new Point(3, 8), Length = 2, Direction = Direction.TopToBottom },
                    new() { Place = new Point(4, 4), Length = 2, Direction = Direction.LeftToRight },
                    new() { Place = new Point(1, 4), Length = 2, Direction = Direction.TopToBottom },

                    new() { Place = new Point(0, 7), Length = 3, Direction = Direction.LeftToRight },
                    new() { Place = new Point(7, 3), Length = 3, Direction = Direction.TopToBottom },

                    new() { Place = new Point(2, 2), Length = 4, Direction = Direction.TopToBottom }
                },
            });
        }

        public IEnumerable<ShipData> Generate()
        {
            var value = enumerator.Current;
            if (!enumerator.MoveNext())
                enumerator.Reset();
            return value;
        }

        class RandomPresetIterator : IEnumerator<IEnumerable<ShipData>>
        {
            private IEnumerable<IEnumerable<ShipData>> startData;

            List<IEnumerable<ShipData>> presets = new List<IEnumerable<ShipData>>();

            public RandomPresetIterator(IEnumerable<IEnumerable<ShipData>> presets)
            {
                startData = presets.ToList();
                this.presets = presets.ToList();

                TakeRandom();
            }

            private IEnumerable<ShipData> _current = null!;

            public IEnumerable<ShipData> Current => _current;

            object IEnumerator.Current => _current;

            public void Dispose()
            {
                startData = null;
                presets = null;
            }

            public bool MoveNext()
            {
                if (presets.Count == 0)
                    return false;
                TakeRandom();
                return true;
            }

            private void TakeRandom()
            {
                var index = Random.Shared.Next(presets.Count);
                _current = presets[index];
                presets.RemoveAt(index);
            }

            public void Reset()
            {
                presets = startData.ToList();
            }
        }
    }
}
