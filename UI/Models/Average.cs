using DAL.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class Average
    {
        public static List<ExamAvg> AverageExam(long classroomID)
        {
            BasExamContext context = new BasExamContext();
            List<Exam> examList = context.Exams.Where(x => x.ClassroomID == classroomID).ToList();
            List<ExamOfStudent> examOfStudents;
            ExamAvg examAvg;
            List<ExamAvg> examAvgList = new List<ExamAvg>();

            int temp = 0;
            int studentCount = 0;

            foreach (Exam item in examList)
            {
                examOfStudents = context.ExamOfStudents.Where(x => x.ExamID == item.Id).ToList();
                studentCount = examOfStudents.Count;

                foreach (ExamOfStudent e in examOfStudents)
                {
                    temp += e.Score;
                }
                examAvg = new ExamAvg();
                examAvg.examName = item.Name;
                examAvg.average = temp / studentCount;
                examAvgList.Add(examAvg);
            }

            return examAvgList;
        }

        public static List<ExamAvg> AverageOfAllExam()
        {
            BasExamContext context = new BasExamContext();
            //List<ExamOfStudent> examOfStudent = context.ExamOfStudents.ToList();
            List<ExamAvg> examAvgList = new List<ExamAvg>();
            ExamAvg examAvg;
            double temp = 0;
            int studentCount = 0;

            string[] examName = { "1.Sınav", "2.Sınav", "3.Sınav", "4.Sınav" };
            for (int i = 0; i < examName.Length; i++)
            {
                foreach (Exam item in context.Exams.Where(x => x.Name == examName[i]).ToList())
                {
                    foreach (ExamOfStudent exofstudent in item.ExamOfStudents.ToList())
                    {
                        temp += exofstudent.Score;
                        studentCount++;
                    }
                }
                examAvg = new ExamAvg();
                examAvg.average = temp/studentCount;
                examAvg.examName = examName[i];
                examAvgList.Add(examAvg);
                temp = 0;
                studentCount = 0;
            }

            return examAvgList;
        }
    }
}