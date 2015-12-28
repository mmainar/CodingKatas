///<reference path="~/dependencies/jasmine.js"/>
///<reference path="~/sources/Calculator.js"/>

describe("Calculator", function () {
    var calculator = new Calculator();

    it("should multiple two positive numbers", function () {
        var result = calculator.multiple(2, 5);

        expect(result).toBe(10);
    });
});