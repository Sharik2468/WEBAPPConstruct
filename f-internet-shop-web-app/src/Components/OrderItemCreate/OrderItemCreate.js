import React from 'react'
const OrderItemCreate = ({ addOrderItem }) => {
    const handleSubmit = (e) => {
        e.preventDefault()
        const { value } = e.target.elements.url
        const OrderItem = { url: value }

        // Изменить значение поля amount_Order_Item на 2
        OrderItem.amount_Order_Item = 2;


        const createOrderItem = async () => {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(OrderItem)
            }
            const response = await fetch("https://localhost:7194/api/OrderItem/",

                requestOptions)

            return await response.json()
                .then((data) => {
                    console.log(data)
                    // response.status === 201 && addOrderItem(data)
                    if (response.ok) {
                        addOrderItem(data)
                        e.target.elements.url.value = ""
                    }
                },
                    (error) => console.log(error)
                )
        }
        createOrderItem()
    }
    return (
        <React.Fragment>
            <h3>Создание новой строки заказа</h3>
            <form onSubmit={handleSubmit}>
                <label>URL: </label>
                <input type="text" name="url" placeholder="Введите Url:" />
                <button type="submit">Создать</button>
            </form>
        </React.Fragment >
    )
}
export default OrderItemCreate

