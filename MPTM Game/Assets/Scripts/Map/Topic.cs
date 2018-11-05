using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topic{
    public static List<Topic> topics = new List<Topic>(); //If topic is not equal to topic in question, create a new topic


    public string topicName;
    public List<string> topicInfo;
    public string topicString; //string of all the info
    public Topic(string _topicName, List<string> _topicInfo)
    {
        this.topicName = _topicName;
        this.topicInfo = _topicInfo;
    }

   
    
}
