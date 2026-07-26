using UnityEngine;
using System;
using TMPro;

public class OfficeGame : MonoBehaviour
{


    /** To-dos
        Generate number of balance
        Generate 2 columns of numbers that sum to that balance
        Generate position swaps
        Display column sums
        Display number of swaps to do
        Upon balance add money and load new puzzle
        Leave minigame

    */

    Transform grid;
    UniversalOverlayScript currency_holder;

    [SerializeField] int max_balance;

    TextMeshProUGUI left_col_display;
    TextMeshProUGUI right_col_display;
    TextMeshProUGUI swaps_display;

    int swaps;
    int balance_value;
    int[] card_values = new int[8]; // 8 values to balance the books - initial state

    int[] current_state = new int[8]; //keeps current board state - not accurate forgot to account for how cards move in response to drags
    


    [SerializeField] GameObject card; //used to instantiate cards for the puzzle

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = transform.GetChild(0);
        left_col_display = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        right_col_display = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        swaps_display = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        currency_holder = GameObject.FindAnyObjectByType<UniversalOverlayScript>();
        CreatePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePuzzle()
    {
        balance_value = (int)UnityEngine.Random.Range(25f, max_balance); //get correct value
        //get individual card values
        int running_balance = balance_value;
        card_values[0] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[0];
        card_values[2] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[2];
        card_values[4] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[4];
        card_values[6] = running_balance;
        //second column
        running_balance = balance_value;
        card_values[1] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[1];
        card_values[3] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[3];
        card_values[5] = (int)UnityEngine.Random.Range(1,Math.Min(running_balance-3, 778));
        running_balance -= card_values[5];
        card_values[7] = running_balance;

        //populate initial state
        for (int i = 0; i < 8; i++)
        {
            current_state[i] = card_values[i];
        }

        int iterations = 0;
        while(CheckIfWon()) {
        //swap cards
            float num_of_swaps_determiner = UnityEngine.Random.Range(0, 10);
            swaps = 1;
            if (num_of_swaps_determiner > 7) swaps = 2;
            //if (num_of_swaps_determiner > 9) swaps = 3; Already Complicated enough

            int index1 = (int)UnityEngine.Random.Range(0, 8);
            int index2 = (int)UnityEngine.Random.Range(0, 8);
            SwapCards(index1, index2);

            iterations++;
            if(iterations > 10) {Debug.Log("too many swaps"); return;}
        }

        //column displays
        left_col_display.text = LeftColSum().ToString();
        right_col_display.text = RightColSum().ToString();
        swaps_display.text = "Swaps to make puzzle" + swaps.ToString();

        //make cards - after swaps to avoid messy movement code being required to set up puzzle, no need to figure out swapping positioning(probably w/ SetSiblingIndex if it becomes necessary)
        foreach(int value in current_state) {
            GameObject new_card = Instantiate(card);
            new_card.transform.SetParent(grid);
            new_card.transform.localScale = new Vector3(1,1,1);
            SetCardText(value.ToString(), new_card);
        }
        
        
    }

    private void SetCardText(String text, GameObject card)
    {
        TextMeshProUGUI num_display = card.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        num_display.text = text;
    }

    //switch cards
    private void SwapCards(int index1, int index2)
    {
        int temp = current_state[index2];
        current_state[index2] = current_state[index1];
        if (index2 < index1) {
            for(int i=index1; i > index2 + 1; i--)
            {
                current_state[i] = current_state[i - 1];
            }
            current_state[index2+1] = temp;
        }
        if (index2 > index1)
        {
            for (int i = index1; i < index2-1; i++)
            {
                current_state[i] = current_state[i+1];
            }
            current_state[index2-1] = temp;
        }
    
    }

    public void UpdateGame(int start_move_index, int end_move_index)
    {
        Debug.Log("GameUpdate");
        SwapCards(start_move_index, end_move_index);
                //column displays
        left_col_display.text = LeftColSum().ToString();
        right_col_display.text = RightColSum().ToString();
        if (CheckIfWon())
        {
            currency_holder.ChangeMoney(55);
            currency_holder.ChangeHappiness(-.5f);
            ClearGame();
            CreatePuzzle();
        }
    }

    public bool CheckIfWon()
    {
        int left_col=LeftColSum();
        int right_col=RightColSum();

        return left_col == right_col;
        
    }

    private int LeftColSum()
    {
        int left_col = 0;
        for(int i =0; i < 8; i+=2)
        {
            left_col += current_state[i];
        }
        return left_col;
    }

    private int RightColSum()
    {
        int right_col = 0;
        for(int i =1; i < 8; i+=2)
        {
            right_col += current_state[i];
        }
        return right_col;
    }

    private void ClearGame()
    {
        //formatted this way to avoid updating indexes or not as objects get destroyed
        GameObject[] cards = new GameObject[8];
        for(int i = 0; i < 8; i++)
        {
            cards[i] = grid.GetChild(i).gameObject;
        }
        for(int i = 0; i < 8; i++)
        {
            Destroy(cards[i]);
        }
    }
}
