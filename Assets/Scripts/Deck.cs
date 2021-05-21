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
    public Button apuesta10Button;
    public Button apuesta100Button;
    public Text BancaMessage;
    public Text apuestaMessage;
    public Text finalMessage;
    public Text probMessage;
    public Text probMessage1;
    public Text probMessage2;


    public int[] values = new int[52];
    int cardIndex = 0;

    int valuesPlayer=0;
    int valuesDealer=0;
    int[] cardsPlayer = new int [50];
    int[] cardsDealer = new int [50];
    int round = 0;

    int banca = 1000;
    int apuesta = 0;

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

                if (o+1 >10)
                {
                    values[u] = 10;
                    u++;
                }
                else
                {

                    values[u] = o + 1;
                    u++;
                }
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
        apuesta = 0;
        actualizarBanca();
        for (int i = 0; i < 2; i++)
        {
            PushDealer();
            PushPlayer();
            round++;
            
        }
        /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        if (valuesPlayer == 21)
        {
            finalMessage.text = "Blacjack! Has GANADO :D";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;
            banca += apuesta * 2;
            apuesta = 0;
            actualizarBanca();
        } else if (valuesDealer == 21)
        {
            finalMessage.text = "Blacjack! Has perdido :(";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;
            banca += 0;
            apuesta = 0;
            actualizarBanca();
        }
        if (valuesPlayer > 21)
        {
            finalMessage.text = "Te has pasado, has perdido :(";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;
            banca += 0;
            apuesta = 0;
            actualizarBanca();
        }
        else if (valuesDealer > 21)
        {
            finalMessage.text = "El dealer se ha pasado, has ganado :)";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;

            banca += apuesta * 2;
            apuesta = 0;
            actualizarBanca();
        }
    }

    private void CalculateProbabilities()
    {
        float probabilidad;
        int casosPosibles;
        //- Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
        if (round != 0)
        {
            int valoresVisiblesDealer = valuesDealer - cardsDealer[0];
            casosPosibles = 13 - valuesPlayer + valoresVisiblesDealer;
            probabilidad = casosPosibles/ 13f;
            if (probabilidad > 1)
            {
                probabilidad = 1;
            }else if (probabilidad < 0)
            {
                probabilidad = 0;
            }

            if((valuesPlayer - valoresVisiblesDealer) >= 10)
            {
                probabilidad = 0;
            }

            probMessage.text = (probabilidad * 100).ToString() + " %";

        }

        //Probabilidad de que el jugador obtenga más de 21 si pide una carta

        float probabilidad2;
        int casosPosibles2;
        casosPosibles2 = 13 - (21 - valuesPlayer);
        probabilidad2 = casosPosibles2 / 13f;
        if (probabilidad2 > 1)
        {
            probabilidad2 = 1;
        }
        else if (probabilidad2 < 0)
        {
            probabilidad2 = 0;
        }
        if (valuesPlayer < 12)
        {
            probabilidad2 = 0;
        }
        probMessage1.text = (probabilidad2 * 100).ToString() + " %";

        // Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta

        float probabilidadLlegarA17;
        int casosPosiblesHasta17;
        casosPosiblesHasta17 = 13 - (16 - valuesPlayer);
        probabilidadLlegarA17 = casosPosiblesHasta17 / 13f;
        if (probabilidadLlegarA17 > 1)
        {
            probabilidadLlegarA17 = 1;
        }
        else if (probabilidadLlegarA17 < 0)
        {
            probabilidadLlegarA17 = 0;
        }
        if (valuesPlayer < 7)
        {
            probabilidadLlegarA17 = 0;
        }


        float probabilidadEntre17y21 = probabilidadLlegarA17 - probabilidad2;
        if (probabilidadEntre17y21 > 1)
        {
            probabilidadEntre17y21 = 1;
        }
        else if (probabilidadEntre17y21 < 0)
        {
            probabilidadEntre17y21 = 0;
        }

        probMessage2.text = (probabilidadEntre17y21 * 100).ToString() + " %";

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
        cardsDealer[round] = values[cardIndex];
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        valuesPlayer += values[cardIndex];
        cardsPlayer[round] = values[cardIndex];
        cardIndex++;
        Debug.Log(valuesPlayer);
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        if (valuesPlayer > 21)
        {
            finalMessage.text = "Te has pasado, has perdido :(";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;

            banca +=0;
            apuesta = 0;
            actualizarBanca();
        }
        else if (valuesPlayer == 21)
        {
            finalMessage.text = "Blacjack! Has GANADO :D";
            stickButton.interactable = false;
            hitButton.interactable = false;
            apuesta100Button.interactable = false;
            apuesta10Button.interactable = false;

            banca += apuesta * 2;
            apuesta = 0;
            actualizarBanca();
        }
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
        apuesta100Button.interactable = false;
        apuesta10Button.interactable = false;
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        while (valuesDealer <= 16)
        {
            PushDealer();
        }
        //puntosDealer.text = dealer.GetComponent<CardHand>().points.ToString();

        if (valuesDealer == 21)
        {
            finalMessage.text = "Blacjack! Has perdido :(";

            banca += 0;
            apuesta = 0;
            actualizarBanca();
        }
        else if (valuesDealer > 21)
        {
            finalMessage.text = "El dealer se ha pasado, has ganado :)";

            banca += apuesta * 2;
            apuesta = 0;
            actualizarBanca();
        }
        else if (valuesDealer < valuesPlayer)
        {
            finalMessage.text = "Has ganado :)";

            banca += apuesta * 2;
            apuesta = 0;
            actualizarBanca();
        }
        else if (valuesDealer == valuesPlayer)
        {
            finalMessage.text = "Has empatado :/";

            banca += apuesta;
            apuesta = 0;
            actualizarBanca();
        }
        else
        {
            finalMessage.text = "Has perdido :(";

            banca += 0;
            apuesta = 0;
            actualizarBanca();
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
        apuesta100Button.interactable = true;
        apuesta10Button.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        valuesPlayer=0;
        round = 0;
        valuesDealer = 0;
        ShuffleCards();
        StartGame();
    }

    public void Ap10()
    {
        if(banca > 10)
        {

            apuesta += 10;
            banca -= 10;
            actualizarBanca();
        }
    }
    public void Ap100()
    {
        if(banca > 100)
        {
            apuesta += 100;
            banca -= 100;
            actualizarBanca();
        }
    }

    private void actualizarBanca()
    {
        apuestaMessage.text = apuesta.ToString();
        BancaMessage.text = banca.ToString();
    }
}
