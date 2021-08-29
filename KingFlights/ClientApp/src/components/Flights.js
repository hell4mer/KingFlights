import React, { Component } from 'react';

export class Flights extends Component {
    static displayName = Flights.name;

    constructor(props) {
        super(props);
        this.state = { flights: [], loading: true };

        this.handleSearch = this.handleSearch.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        this.populateFlightsData();
    }

    handleSearch(e) {
        e.preventDefault();
        this.populateFlightsData();
    }

    componentDidMount() {
        this.populateFlightsData();
    }

    static searchParamsBox() {
        return (
            <form class="form-inline" onSubmit={event => this.handleSubmit(event)}>
                <div class="form-group">
                    <label for="currency">Currency:</label>
                    <input id="currency" name="currency" class="form-control"/>
                </div>
                <div class="form-group">
                    <label for="departureAirport">Departure airport: </label>
                    <input id="departureAirport" name="departureAirport" class="form-control"/>
                </div>
                <button id="searchBtn" class="btn btn-default" onClick={this.handleSearch}>Search</button>
            </form>
        );

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
                            <td>{flight.departureDate}</td>
                            <td>{flight.transfers}</td>
                            <td>{flight.passengers}</td>
                            <td>{flight.price + " " + flight.currency}</td>
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
                <h1 id="tabelLabel" >Flights</h1>
                <p>List of flights</p>

                {Flights.searchParamsBox()}
                <br/>
                {contents}
            </div>
        );
    }

    async populateFlightsData() {
        //var urlParams = new URLSearchParams({
        //    currency: document.getElementById('currency').value
        //});

        const response = await fetch('flights/get', { //+ urlParams);
            method: 'POST'
        });
        const data = await response.json();
        this.setState({ flights: data, loading: false });
    }
}
