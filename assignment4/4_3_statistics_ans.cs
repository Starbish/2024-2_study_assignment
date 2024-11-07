using System;
using System.Linq;

namespace statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,] data = {
                {"StdNum", "Name", "Math", "Science", "English"},
                {"1001", "Alice", "85", "90", "78"},
                {"1002", "Bob", "92", "88", "84"},
                {"1003", "Charlie", "79", "85", "88"},
                {"1004", "David", "94", "76", "92"},
                {"1005", "Eve", "72", "95", "89"}
            };
            // You can convert string to double by
            // double.Parse(str)

            int stdCount = data.GetLength(0) - 1;
            // ---------- TODO ----------

            double[] total_score = new double[stdCount];
            double[] average = new double[3];
            double[] min = new double[3];
            double[] max = new double[3];

            for(int i = 0; i < 3; i++) {
                for(int j = 1; j < stdCount+1; j++) {
                    double score = double.Parse(data[j, i+2]);
                    Console.WriteLine(score);

                    // min, max 갱신
                    if(min[i] > score || min[i] == 0.0)
                        min[i] = score;
                    
                    if(max[i] < score)
                        max[i] = score;
                    
                    // 과목 당 평균을 계산하기 위해 값을 저장함
                    average[i] += score;

                    // 개인 총점
                    total_score[j-1] += score;
                }

                // 평균 계산
                average[i] /= stdCount;
            }

            // 결과 출력
            Console.WriteLine("Average Scores:");
            for(int i = 0; i < 3; i++)
                Console.WriteLine($"{data[0, i+2]}: {average[i]:F2}");

            Console.WriteLine("\nMax and min Scores:");
            for(int i = 0; i < 3; i++)
                Console.WriteLine($"{data[0, i+2]}: ({max[i]}, {min[i]})");

            Console.WriteLine("\nStudents rank by total scores:");
            // n!은 좀..
            // Array.Sort 를 이용해서 내림차순으로 정렬하는 경우 값만 정렬되므로 원래의 인덱스를 같이 저장해야 함
            var score_list = total_score.Select((score, index) => new {score, index}).ToArray();
            Array.Sort(score_list, (param1, param2) => param2.score.CompareTo(param1.score));
            
            // 더 좋은 방법이 있을 것 같은데..
            for(int i = 1; i <= stdCount; i++) {

                int rank = Array.FindIndex(score_list, item => item.index == i-1);
                Console.WriteLine($"{data[i, 1]}: {GetRankString(rank)}");
            }
            // --------------------
        }

        static string GetRankString(int rank) {

            if(rank == 0)
                return "1st";
            else if (rank == 1)
                return "2nd";
            else if (rank == 2)
                return "3rd";
                
            return $"{rank+1}th";
        }
    }
}

/* example output

Average Scores: 
Math: 84.40
Science: 86.80
English: 86.20

Max and min Scores: 
Math: (94, 72)
Science: (95, 76)
English: (92, 78)

Students rank by total scores:
Alice: 4th
Bob: 1st
Charlie: 5th
David: 2nd
Eve: 3rd

*/
