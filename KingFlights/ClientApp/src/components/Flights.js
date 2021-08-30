import React, { Component } from 'react';

export class Flights extends Component {
    static displayName = Flights.name;

    constructor(props) {
        super(props);
        this.state = { flights: [], loading: true };

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        
        e.preventDefault();
        this.populateFlightsData();
    }

    componentDidMount() {
        //this.populateFlightsData();
    }

    static renderFlightsTable(flights) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Origin</th>
                        <th>Destination</th>
                        <th>Date</th>
                        <th>Transfers</th>
                        <th>Passengers</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {flights.map(flight =>
                        <tr key={flight.id}>
                            <td>{flight.originLocationCode}</td>
                            <td>{flight.destinationLocationCode}</td>
                            <td>{new Date(flight.departureDate).toLocaleDateString('hr-HR')}</td>
                            <td>{flight.transfers}</td>
                            <td>{flight.passengers}</td>
                            <td>{flight.price + " " + flight.currencyCode}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Flights.renderFlightsTable(this.state.flights);

        return (
            <div>
                <h1 id="tabelLabel" >Low-cost flights search</h1>

                <form onSubmit={this.handleSubmit}>
                    <div class="form-group">
                        <label for="originLocationCode">Origin location:</label>
                        <input id="originLocationCode" type="text" name="originLocationCode" maxLength="3" class="form-control text-uppercase" />
                    </div>
                    <div class="form-group">
                        <label for="destinationLocationCode">Destination location:</label>
                        <input id="destinationLocationCode" type="text" name="destinationLocationCode" maxLength="3" class="form-control text-uppercase" />
                    </div>
                    <div class="form-group">
                        <label for="departureDate">Departure:</label>
                        <input id="departureDate" name="departureDate" type="date" placeholder="Departure?" class="form-control" />
                    </div>
                    <div class="form-group">
                        <label for="returnDate">Date of return:</label>
                        <input id="returnDate" name="returnDate" type="date" placeholder="Return?" class="form-control" />
                    </div>
                    
                    

                    <div class="form-group">
                        <label for="passengers">Passengers:</label>
                        <select id="passengers" name="passengers" class="form-control input-sm">
                            <option>1</option>
                            <option>2</option>
                            <option>3</option>
                            <option>4</option>
                            <option>5</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="currencyCode">Currency:</label>
                        <select id="currencyCode" name="currencyCode" class="form-control input-sm">
                            <option>USD</option>
                            <option>EUR</option>
                            <option>HRK</option>
                        </select>
                    </div>
                    <input type="submit" class="btn btn-default" value="Search" />
                </form>

                <br />
                {contents}
            </div>
        );
    }

    async populateFlightsData() {
        var urlParams = new URLSearchParams({
            originLocationCode: document.getElementById('originLocationCode').value,
            destinationLocationCode: document.getElementById('destinationLocationCode').value,
            departureDate: document.getElementById('departureDate').value,
            returnDate: document.getElementById('returnDate').value,
            passengers: document.getElementById('passengers').value,
            currencyCode: document.getElementById('currencyCode').value
        });

        const response = await fetch('flights/GetCheapFlights?' + urlParams);
        const data = await response.json();
        this.setState({ flights: data, loading: false });
    }
}
