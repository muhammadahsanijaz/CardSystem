# CardSystem
Contains the card system implemented with inheritance. The card has generic properties of equipping to slot, staking, and merging with proper implementation.
Filters are implemented to Get Data from the lists of cards respective to UI filter and other properties like states and arts.
This is implemented in the ECS system.

# Abstract
Contains Cards abstract model and 2 top models of entity and property type regarding physical properties and inner properties of card respectively.
Contains Vehicle Data model for fully formed single preset which contains VehicleCard, DriverCard, 3 Accessories Cards, 4 Tech Cards, and up to 5 PowerupCards

# Categroies
Base Card models for every Card Type we have in the game.

# Handlers
CardHandler is a Component that handles interaction with the Systems.
SlotHandler is a Component used for showing when the card is equipped or staked in the respective system. I have 2 inherited Types to handle processes when interacting with the respective systems.

# Cores
CardManager is the system that contains all the cards and references to the factory and card filters
CardFactory is the component to process card filters
CardFilters (Correct Name should be CardHandlerFactory) CardFactory is the component to process cardsHandlers filters

