import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>KingFlights</h1>
                <p>Find cheapest flights fast!</p>
                <p>To get started, go to <a href='/flights'>our search form</a>.</p>
            </div>
        );
    }
}
