import cup_of_coffee_without_pair from "../assets/items/CupOfCoffeeWithoutPair.webp";


function CupOfCoffee() {
    return (
        <div className="cup-of-coffee-container">
            <img src={cup_of_coffee_without_pair} alt="CupOfCoffeeWithoutPair" className="cup-of-coffee-without-pair" />
        </div>
    );
}

export default CupOfCoffee;