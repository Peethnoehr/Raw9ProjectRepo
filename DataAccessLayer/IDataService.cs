using System;
using System.Collections.Generic;
using StackOverFlow;

namespace DataAccessLayer
{
    public interface IDataService
    {
        User GetUser(string username);
        List<User> GetUsers();
        User CreateUser(string username, string password, string email, string salt, DateTime date);
        Boolean UpdateUser(string username, string password, string email, string salt);
        Boolean DeleteUser(string username);
        Marking GetMarking(int idmarking);
        List<Marking> GetMarkings(string username);
        Boolean UpdateMarking(int markid, string annotation);
        Marking CreateMarking(string annotation, string username, int? postid, int? commentid, DateTime date);
        Boolean DeleteMarking(int markid);
        SearchHistory GetSearchHistory(int searchhistoryid);
        List<SearchHistory> GetSearchHistories(string username);
        SearchHistory CreateSearchHistory(string text, string username, DateTime date);
        Boolean DeleteSearchHistory(int searchhistoryid);
        List<Comment> GetComments(int postid);
        List<Tag> GetTags(int postid);
        Question GetDetailQuestion(int postid);
        List<Post> GetAnswers(int postid);
        Answer getAnswer(int postid);
        List<Post> searchPosts(string searchtext);
    }
}