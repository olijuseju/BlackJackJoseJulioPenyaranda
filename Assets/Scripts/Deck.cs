using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;

    int valuesPlayer=0;
    int valuesDealer=0;
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        int u = 0;
        for(int i = 0; i < 4; i++)
        {
            for (int o = 0; o < 13; o++)
            {
                values[u] = o+1;
                u++;
            }
        }

        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
    }

    private void ShuffleCards()
    {
        int randomNumber;
        Sprite tempFaces;
        int tempValues;
        for(int i = 0; i < 52; i++)
        {
            randomNumber = Random.Range(0, 52);
            tempFaces = faces[0];
            faces[0] = faces[randomNumber];
            faces[randomNumber] = tempFaces;
            tempValues = values[0];
            values[0] = values[randomNumber];
            values[randomNumber] = tempValues;
        }

        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        Debug.Log(cardIndex);
        float probabilidad;
        //- Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
        if (cardIndex ==3)
        {
            probabilidad = 1/4;
            Debug.Log(probabilidad.ToString());
            Debug.Log("A ngel maric000n      " + valuesPlayer);


        }
        else if (cardIndex > 3)
        {
            int puntuacionDescubierta = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
            Debug.Log("A ngel maric000n      " + valuesDealer);
        }



        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        valuesDealer += values[cardIndex];

        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        valuesPlayer += values[cardIndex];
        cardIndex++;
        //Debug.Log(valuesPlayer);
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();
        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        hitButton.interactable = false;
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        //puntosDealer.text = dealer.GetComponent<CardHand>().points.ToString();

        if (dealer.GetComponent<CardHand>().points == 21)
        {
            finalMessage.text = "Blacjack! Has perdido :(";
        }
        else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has ganado :)";
        }
        else if (dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has ganado :)";
        }
        else if (dealer.GetComponent<CardHand>().points == player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has empatado :/";
        }
        else
        {
            finalMessage.text = "Has perdido :(";
        }
        stickButton.interactable = false;

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */

    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
