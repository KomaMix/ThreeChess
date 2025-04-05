using System.Collections.Generic;

namespace ThreeChess.Services
{
    public class MoveLogicalElementsService
    {
        public List<List<string>> GetDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.AddRange(GetLeftDiagonals());

            diagonals.AddRange(GetLeftBottomDiagonals());

            diagonals.AddRange(GetRightBottomDiagonals());

            diagonals.AddRange(GetRightDiagonals());

            diagonals.AddRange(GetRightUpDiagonals());

            diagonals.AddRange(GetLeftUpDiagonals());

            return diagonals;
        }

        private List<List<string>> GetLeftDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.Add(new List<string>
            {
                "G1", "H2"
            });

            diagonals.Add(new List<string>
            {
                "F1", "G2", "H3"
            });

            diagonals.Add(new List<string>
            {
                "E1", "F2", "G3", "H4"
            });

            diagonals.Add(new List<string>
            {
                "D1", "E2", "F3", "G4", "H9"
            });

            diagonals.Add(new List<string>
            {
                "C1", "D2", "E3", "F4", "G9", "H10"
            });

            diagonals.Add(new List<string>
            {
                "B1", "C2", "D3", "E4", "F9", "G10", "H11"
            });

            return diagonals;
        }

        private List<List<string>> GetLeftBottomDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.Add(new List<string>
            {
                "H11", "G12"
            });

            diagonals.Add(new List<string>
            {
                "H10", "G11", "F12"
            });

            diagonals.Add(new List<string>
            {
                "H9", "G10", "F11", "E12"
            });

            diagonals.Add(new List<string>
            {
                "H4", "G9", "F10", "E11", "I12"
            });

            diagonals.Add(new List<string>
            {
                "H3", "G4", "F9", "E10", "I11", "J12"
            });

            diagonals.Add(new List<string>
            {
                "H2", "G3", "F4", "E9", "I10", "J11", "K12"
            });


            return diagonals;
        }

        private List<List<string>> GetRightBottomDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.Add(new List<string>
            {
                "K12", "L11"
            });

            diagonals.Add(new List<string>
            {
                "J12", "K11", "L10"
            });

            diagonals.Add(new List<string>
            {
                "I12", "J11", "K10", "L9"
            });

            diagonals.Add(new List<string>
            {
                "E12", "I11", "J10", "K9", "L5"
            });

            diagonals.Add(new List<string>
            {
                "F12", "E11", "I10", "J9", "K5", "L6"
            });

            diagonals.Add(new List<string>
            {
                "G12", "F11", "E10", "I9", "J5", "K6", "L7"
            });

            return diagonals;
        }

        private List<List<string>> GetRightDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.Add(new List<string>
            {
                "L7", "K8"
            });

            diagonals.Add(new List<string>
            {
                "L6", "K7", "J8"
            });

            diagonals.Add(new List<string>
            {
                "L5", "K6", "J7", "I8"
            });

            diagonals.Add(new List<string>
            {
                "L9", "K5", "J6", "I7", "D8"
            });

            diagonals.Add(new List<string>
            {
                "L10", "K9", "J5", "I6", "D7", "C8"
            });

            diagonals.Add(new List<string>
            {
                "L11", "K10", "J9", "I5", "D6", "C7", "B8"
            });

            return diagonals;
        }

        private List<List<string>> GetRightUpDiagonals()
        {
            var diagonals = new List<List<string>>();

            string alphs = "ABCDIJK";
            string nums = "2345678";
            

            for (int i = 2; i < 8; i++)
            {
                List<string> ids = new List<string>();

                for (int j = 0; j < i; j++)
                {
                    ids.Add(alphs[j].ToString() + nums[nums.Length - i + j].ToString());
                }

                diagonals.Add(ids);
            }

            return diagonals;
        }

        private List<List<string>> GetLeftUpDiagonals()
        {
            var diagonals = new List<List<string>>();

            diagonals.Add(new List<string>
            {
                "A2", "B1"
            });

            diagonals.Add(new List<string>
            {
                "A3", "B2", "C1"
            });

            diagonals.Add(new List<string>
            {
                "A4", "B3", "C2", "D1"
            });

            diagonals.Add(new List<string>
            {
                "A5", "B4", "C3", "D2", "E1"
            });

            diagonals.Add(new List<string>
            {
                "A6", "B5", "C4", "D3", "E2", "F1"
            });

            diagonals.Add(new List<string>
            {
                "A7", "B6", "C5", "D4", "E3", "F2", "G1"
            });

            return diagonals;
        }
    }
}
