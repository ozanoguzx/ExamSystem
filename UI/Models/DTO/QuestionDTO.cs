using DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models.DTO
{
    public class QuestionDTO
    {
        public List<QuestionDTOVM> QuestionToDTO(List<Question> items)
        {
            List<QuestionDTOVM> questionList = new List<QuestionDTOVM>();
            QuestionDTOVM questionVM;
            foreach (Question item in items)
            {
                questionVM = new QuestionDTOVM();
                questionVM.Id = item.Id;
                questionVM.Content = item.Content;
                //questionVM.Type = item.Type;
                //questionVM.UserID = item.UserID;
                questionVM.SubjectID = item.SubjectID;
                questionVM.A = item.A;
                questionVM.B = item.B;
                questionVM.C = item.C;
                questionVM.D = item.D;
                questionVM.E = item.E;
                questionList.Add(questionVM);
            }
            return questionList;
        }
    }
}