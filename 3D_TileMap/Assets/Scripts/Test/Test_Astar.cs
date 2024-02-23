using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Astar : TestBase
{
    void Start()
    {
        Node node1 = new Node(1,11);
        Node node2 = new Node(1,11);

        List<Node> nodes = new List<Node>();

        //int i = 0;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {

        List <int> list = new List<int>();
        list.Add(3);
        list.Add(1);
        list.Add(2);
        list.Add(4);
        list.Add(5);

        list.Sort();
        //int i = 0;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {

        Node node1 = new Node(10, 20);
        node1.G = 10;
        Node node2 = new Node(10, 20);
        node2.G = 20;
        Node node3 = new Node(10, 20);
        node3.G = 30;
        Node node4 = new Node(10, 20);
        node4.G = 40;
        Node node5 = new Node(10, 20);
        node5.G = 50;

        List<Node> lists = new List<Node>();

        lists.Add(node1);
        lists.Add(node2);
        lists.Add(node3);
        lists.Add(node4);
        lists.Add(node5);

        lists.Sort();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // testData 정렬확인
        Test_Data t1 = new Test_Data(0, 5.0f, "a");
        Test_Data t2 = new Test_Data(2, 1.0f, "d");
        Test_Data t3 = new Test_Data(1, 4.0f, "e");
        Test_Data t4 = new Test_Data(3, 2.0f, "c");
        Test_Data t5 = new Test_Data(4, 3.0f, "e");

        List<Test_Data> nodes = new List<Test_Data>();

        nodes.Add(t1);
        nodes.Add(t2);
        nodes.Add(t3);
        nodes.Add(t4);
        nodes.Add(t5);

        nodes.Sort();

    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        GridMap grid = new GridMap(3, 3);

    }
}
