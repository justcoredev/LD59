using System.Collections;
using UnityEngine;

public class CardGiver : MonoBehaviour
{
    public GameObject cardPrefab;
    public float cardForce = 10.0f;

    public void GiveCards(int quantity, float delay = 0.33f)
    {
        StartCoroutine(GiveCardsRoutine(quantity, delay));
    }

    IEnumerator GiveCardsRoutine(int quantity, float delay)
    {
        for(int i = 0; i < quantity; i++)
        {
            var cardObj = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            Card card = cardObj.GetComponent<Card>();

            card.ApplyForceImpulse(
                Vector2.right + 
                new Vector2(
                    Random.Range(-1.0f, 10.0f) * 0.05f, 
                    Random.Range(-1.0f, 2.0f) * 0.05f), 
                cardForce
            );

            yield return new WaitForSeconds(delay);
        }
    }
}