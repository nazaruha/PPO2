import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ICustomerItem } from "../../Customer/types";
import http from "../../../../http_common";
import { IOrderCreateItem } from "../types";
import { useFormik } from "formik";
import { OrderCreateSchema } from "../validation";
import classNames from "classnames";
import { IManufacturerItem, IManufacturerServiceResponse } from "../../Manufacturer/types";
import { IProductItem } from "../../Product/types";

const OrderCreatePage = () => {
    const navigate = useNavigate();

    const projectId = localStorage.chosenProjectId;
    const [customers, setCustomers] = useState<ICustomerItem[]>([]);
    const [manufacturers, setManufacturers] = useState<IManufacturerItem[]>([]);
    const [products, setProducts] = useState<IProductItem[]>([]);
    const [productPrice, setProductPrice] = useState<number>(0);
    const [productCount, setProductCount] = useState<number>(0);
    const init: IOrderCreateItem = {
        customerId: 0,
        productId: 0,
        projectId: projectId,
        totalPrice: 0,
        productQuantity: 0,
        sellDate: "",
        manufacturerId: 0,
    };

    const onCreateSubmit = (values: IOrderCreateItem) => {
        console.log("VALUES", values);
        http.post("api/order/create", values)
            .then(resp => {
                console.log(resp);
                navigate("..");
            }).catch(err => {
                console.log("ERR SUBMIT", err);
                if (err.response.data.message.includes("продуктів")) setFieldError("productQuantity", err.response.data.message);
            })
    }

    const formik = useFormik({
        initialValues: init,
        onSubmit: onCreateSubmit,
        validationSchema: OrderCreateSchema
    })

    const { errors, values, touched, handleChange, handleSubmit, setFieldValue, setFieldError } = formik;

    useEffect(() => {
        http.get(`api/customer/byprojectid/${projectId}`)
            .then(resp => {
                console.log("GET CUSTOMERS BY PROJECT", resp);
                setCustomers(resp.data.payload);
            })
            .catch(err => {
                console.log("GET CUSTOMERS BY PROJECT ERR", err);
            })
    }, []);

    useEffect(() => {
        http.get<IManufacturerItem[]>(`api/storage/manufacturer/${projectId}`)
            .then(resp => {
                const { data } = resp;
                console.log("MANUFACTURER LIST", data);
                setManufacturers(data);
                setFieldValue("productQuantity", 0);
                console.log("ManufacturerId", values.manufacturerId);
                if (values.manufacturerId !== 0) {
                    http.get<IProductItem[]>(`api/storage/product/${values.manufacturerId}/${projectId}`)
                        .then(resp => {
                            setFieldValue("productId", 0);
                            // values.productId = 0;
                            console.log("PRODUCTS BY MANUFACTURER ID", resp);
                            setProducts(resp.data);
                        })
                        .catch(err => {
                            console.log("ERR PRODUCTSLST", err);
                        })
                }
            })
            .catch(err => {
                console.log("ERR MANLST", err);
            });
    }, [values.manufacturerId])

    useEffect(() => {
        http.get(`api/storage/product-${values.productId}/project-${projectId}`)
            .then(resp => {
                const { data } = resp;
                console.log("GET CHOSEN PRODUCT DATA", data);
                setProductPrice(data.price);
                setProductCount(data.count);
                setFieldValue("productQuantity", 0);
            }).catch(err => {
                console.log("ERR GET CHOSEN PRODUCT DATA", err);
            })
    }, [values.productId])

    useEffect(() => {
        setFieldValue("totalPrice", values.productQuantity * productPrice);
    }, [values.productQuantity])


    return (
        <>
            <h1 className="text-center">Створення замовлення</h1>

            <form onSubmit={handleSubmit} className="col-md-6 offset-md-3">
                <div className="mb-3">
                    <label htmlFor="customerId" className='form-label'>Оберіть Виробника</label>
                    <select
                        className={classNames(
                            "form-select",
                            { "is-invalid": errors.customerId && touched.customerId }
                        )}
                        aria-label="Category select"
                        defaultValue={values.customerId} // Значення, яке міститься в select
                        onChange={handleChange} // Якщо значення міняється, воно записується у формік
                        name="customerId" // Значення поля у форміку = customerId - якщо його не буде - formik - не буде нічого добавлять
                        id='customerId' // Це використовується для label
                    >
                        {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                        <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                        {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                        {customers.map(item => (
                            <option key={item.id} value={item.id}>{item.firstName} {item.secondName} {item.email}</option>
                        ))}
                    </select>
                    {(errors.customerId && touched.customerId) && (
                        <span className='text-danger'>{errors.customerId}</span>
                    )}
                </div>
                <div className="mb-3 row">
                    <div className="col-md-6">
                        <label htmlFor="manufacturerId" className='form-label'>Оберіть Виробника</label>
                        <select
                            className={classNames(
                                "form-select",
                                { "is-invalid": errors.manufacturerId && touched.manufacturerId }
                            )}
                            aria-label="Manufacturer select"
                            defaultValue={values.manufacturerId} // Значення, яке міститься в select
                            onChange={handleChange} // Якщо значення міняється, воно записується у формік
                            name="manufacturerId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                            id='manufacturerId' // Це використовується для label
                        >
                            {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                            <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                            {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                            {manufacturers.map(item => (
                                <option key={item.id} value={item.id}>{item.name}</option>
                            ))}
                        </select>
                        {(errors.manufacturerId && touched.manufacturerId) && (
                            <span className='text-danger'>{errors.manufacturerId}</span>
                        )}
                    </div>
                    <div className="col-md-6">
                        <label htmlFor="productId" className='form-label'>Оберіть Продукт</label>
                        <select
                            className={classNames(
                                "form-select",
                                { "is-invalid": errors.productId && touched.productId }
                            )}
                            aria-label="Product select"
                            defaultValue={values.productId} // Значення, яке міститься в select
                            value={values.productId}
                            onChange={handleChange} // Якщо значення міняється, воно записується у формік
                            name="productId" // Значення поля у форміку = manufacturerId - якщо його не буде - formik - не буде нічого добавлять
                            id='productId' // Це використовується для label
                        >
                            {/* Значення, яке завжди буде не обране, для того, щоб нагадать, що треба вказувать категорію */}
                            <option value="0" disabled>---</option> {/* disabled - щоб при виборі категорії ти не міг обрати цей варік */}
                            {/* Перебираємо список категорій і виводимо їх у вигляді options */}
                            {products.map(item => (
                                <option key={item.id} value={item.id}>{item.name}</option>
                            ))}
                        </select>
                        {(errors.productId && touched.productId) && (
                            <span className='text-danger'>{errors.productId}</span>
                        )}
                    </div>
                </div>
                <div className="mb-3 input-group">
                    <label htmlFor="productQuantity" className="form-label">Кількість продукту</label>
                    <div className="input-group">
                        <input
                            type="number"
                            className={classNames(
                                "form-control",
                                { "is-invalid": errors.productQuantity && touched.productQuantity }
                            )}
                            id="productQuantity"
                            name="productQuantity"
                            onChange={handleChange}
                            value={values.productQuantity}
                            placeholder="Введіть кількість продукту"
                        />
                        <span className="input-group-text">₴</span>
                        <span className="input-group-text">{values.totalPrice}</span>
                        {errors.productQuantity && touched.productQuantity && (
                            <div className="invalid-feedback">
                                {errors.productQuantity}
                            </div>
                        )}
                    </div>
                </div>

                <div className="mb-3">
                    <label htmlFor="sellDate" className="form-label">Дата продажу</label>
                    <input
                        type="datetime-local"
                        className={classNames(
                            "form-control",
                            { "is-invalid": errors.sellDate && touched.sellDate }
                        )}
                        id="sellDate"
                        name="sellDate"
                        onChange={handleChange}
                        value={values.sellDate.toLocaleString()}
                    />
                    {errors.sellDate && touched.sellDate && (
                        <div className="invalid-feedback">
                            Введіть дату продажі
                        </div>
                    )}
                </div>

                <div className="container d-flex flex-row justify-content-center gap-3">
                    <button type="submit" className="btn btn-primary">Створити</button>
                    <Link to="../" className="btn btn-warning">Відхілити</Link>
                </div>
            </form >

        </>
    )
}

export default OrderCreatePage;