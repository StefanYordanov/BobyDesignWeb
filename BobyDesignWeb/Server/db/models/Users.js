//const sequelizeReq = require('sequelize');

module.exports = (sequelize, DataTypes) => {
    const Users = sequelize.define("Users", {
        
        firstName: {
            type: DataTypes.STRING,
            allowNull: false
        },
        lastName: {
            type: DataTypes.STRING,
            allowNull: false
        },
        userName: {
            type: DataTypes.STRING,
            allowNull: false
        },
        email: {
            type: DataTypes.STRING,
            allowNull: false
        },
        emailConfirmed: {
            type: DataTypes.BOOLEAN,
            allowNull: false
        },
        passwordHash: {
            type: DataTypes.STRING,
            allowNull: false
        }
    })

    return Users;
}