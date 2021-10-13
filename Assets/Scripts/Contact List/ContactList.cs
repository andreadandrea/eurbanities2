using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ContactList : MonoBehaviour
{
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Sprite[] faces;

    private GlobalStateManager_ GSM;

    private int y = 0;
    private void Start()
    {
        GSM = GlobalStateManager_.Instance;
        Simon_OpenPhone_.PhoneOpen += OnPhoneOpen;
        
        var i = 0;
        foreach (Person person in People.Values)
        {
            
            person.face = faces[i];
            i++;
        }
    }

    private void OnPhoneOpen()
    {
        
        foreach (Person person in People.Values)
        {
            
            
            
            if (GSM.GetBool(person.GSMBoolName))
            {
                if (person.alreadyInList) continue;
                
                var newContact = Instantiate
                    (contactPrefab, new Vector3(0, 0, 0), quaternion.identity, gameObject.transform);
                newContact.transform.localPosition = new Vector3(0, y, 0);
                newContact.transform.Rotate(0, 0, 90);
                y -= 110;
                
                newContact.name = person.Name;
                var contact = newContact.GetComponent<Contact>();
            
                contact.nameField.text = person.Name;
                contact.ageField.text = "Age: " + person.Age;
                contact.locationField.text = person.Location;
                contact.occupationField.text = person.Occupation;
                contact.faceField.sprite = person.face;
                
                person.alreadyInList = true;
            }
        }
    }
    
    


    public Dictionary<string, Person> People = new Dictionary<string, Person>()
    {
        { "Abdullah",   new Person("Abdullah",              21, "News Vendor", "Ben's Bikes, Main Street", "abdullahContactList") },
        { "Alexandria", new Person("Alexandria",            35, "Eurbania Mayor", "Town Hall, Mayor's Office", "alexandriaContactList") },
        { "Amin",       new Person("Amin",                  38, "Bar Owner", "Copper Coin Bar, Copper Street", "aminContactList") },
        { "Anna",       new Person("Anna",                  30, "Town Hall Employee", "Town Hall(Office)", "annaContactList") },
        { "Ben",        new Person("Ben",                   65, "Bike Shop Owner", "Ben's Bikes, Main Street", "benContactList") },
        { "Donald",     new Person("Donald",                61, "Former Mayor", "Liberty Park (Center)", "donaldContactList") },
        { "Giorgia",    new Person("Giorgia",               30, "Yoga Instructor", "Liberty Park (East)", "giorgiaContactList") },
        { "Gwen",       new Person("Gwen",                  41, "Bus Driver", "Copper Coin Bar, Copper Street", "gwenContactList") },
        { "Lia",        new Person("Lia",                   8,  "Student", "Pristine Garden, Oak Street", "liaContactList") },
        { "Marta",      new Person("Marta",                 42, "Former Investor, Now Comics Writer", "Liberty Park (East)", "martaContactList") },
        { "Matilda",    new Person("Matilda",               26, "Flower Shop Owner", "Pristine Garden, Oak Street", "matildaContactList") },
        { "Mrs Viveca", new Person("Mrs Viveca",            67, "Retired Teacher", "Oak Street", "vivecaContactList") },
        { "Pablo",      new Person("Pablo",                 45, "Postman", "Everywhere", "pabloContactList") },
        { "Ruben",      new Person("Ruben",                 37, "Journalist", "Copper Coin Bar, Copper Street", "rubenContactList") },
        { "Sigrid",     new Person("Sigrid",                52, "Carpenter", "Law Street", "sigridContactList") },
        { "Siobhan",    new Person("Siobhan",               27, "Gardener", "Liberty Park (Center)", "siobhanContactList") },
        { "Tom",        new Person("Tom",                   50, "Town Hall Employee", "Town Hall (Office)", "tomContactList") }
    };
}

public class Person
{
    public string Name;
    public int Age;
    public string Occupation;
    public string Location;
    public bool Recruited = false;
    public string GSMBoolName;
    public bool alreadyInList = false;
    public Sprite face;

    public Person(string personName, int personAge,string personOccupation, string personLocation, string GSMBoolName)
    {
        Name = personName;
        Age = personAge;
        Occupation = personOccupation;
        Location = personLocation;
            this.GSMBoolName = GSMBoolName;
    }
    
    
}


